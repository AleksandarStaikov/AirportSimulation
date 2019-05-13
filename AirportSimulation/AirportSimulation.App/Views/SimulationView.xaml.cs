namespace AirportSimulation.App.Views
{
    using AirportSimulation.App.HelperMethods;
    using AirportSimulation.App.Helpers;
    using AirportSimulation.App.Resources;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    public partial class SimulationView : UserControl
    {
        private BuildingComponentsHelper _buildingComponentHelper = new BuildingComponentsHelper();

        private List<(int, int)> _usedCells = new List<(int, int)>();
        private List<GridCell> _gridBuildingComponents = new List<GridCell>();

        private BitmapImage _currentBuildingComponentImage;
        private BitmapImage _previousBuildingComponentImage;
        private BitmapImage _mpaBuildingComponentImage;
        private BuildingComponentType _currentBuildingComponentType;

        private (int, int) _lastCoordinates;
        private static int _hintCount;

        private bool _fullPathBuilt = false;

        public SimulationView()
        {
            InitializeComponent();
            _mpaBuildingComponentImage = _buildingComponentHelper.GetBuildingComponentImage(BuildingComponentType.MPA);
        }

        public SimulationGridOptions SimulationGridOptions { get; set; } = new SimulationGridOptions();

        private void SimulationGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid grid))
            {
                return;
            }

            if (_currentBuildingComponentImage == null)
            {
                MessageBox.Show("Choose a component!");
                return;
            }
            
            var (selectedRowIndex, selectedColumnIndex) = GridHelper.GetCurrentlySelectedGridCell(grid, e);
            _lastCoordinates = (selectedRowIndex, selectedColumnIndex);

            if (GridHelper.IsCellAlreadyUsed(grid, _usedCells, _lastCoordinates))
            {
                MessageBox.Show("This cell is already used. Right click to clear it.");
                return;
            }

            var rectangle = RectangleFactory.CreateBuildingComponentRectangle(_currentBuildingComponentImage);
            var gridCellElement = new GridCell
            {
                Element = rectangle,
                Cell = _lastCoordinates,
                ElementType = _currentBuildingComponentType,
                PreviousCell = _usedCells.Count > 0 
                    ? _usedCells[_usedCells.Count - 1] 
                    : Tuple.Create<int?, int?>(null, null).ToValueTuple()
            };

            if (_gridBuildingComponents.Count > 0)
            {
                _gridBuildingComponents[_usedCells.Count - 1].NextCell = gridCellElement.Cell;
            }

            _gridBuildingComponents.Add(gridCellElement);
            _usedCells.Add(_lastCoordinates);
            grid.Children.Add(rectangle);

            Grid.SetRow(rectangle, selectedRowIndex);
            Grid.SetColumn(rectangle, selectedColumnIndex);

            UpdateCanCreateAndCanClearValues(true, true);
            RectangleFactory.RemoveBlinkingRectangles(grid);
            ShowAvailableBuildingComponentPlaces();

            _previousBuildingComponentImage = _currentBuildingComponentImage;
            if (_currentBuildingComponentType != BuildingComponentType.Conveyor)
            {
                _currentBuildingComponentImage = null;
            }

            if (++_hintCount < 2)
            {
                MessageBox.Show("You can place a component only on the indicated places!");
            }
        }

        private void SimulationGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid grid))
            {
                return;
            }

            if (!_usedCells.Any())
            {
                return;
            }

            var cell = GridHelper.GetCurrentlySelectedGridCell(grid, e).ToTuple();
            var buildingComponent = _gridBuildingComponents.FirstOrDefault(x => x.Cell.Item1 == cell.Item1 && x.Cell.Item2 == cell.Item2);
            var indexOfComponentToBeRemoved = _gridBuildingComponents.IndexOf(buildingComponent);

            if (buildingComponent.Created)
            {
                MessageBox.Show("You cannot remove created component!");
                return;
            }

            _gridBuildingComponents.Remove(buildingComponent);
            _usedCells.Remove(cell.ToValueTuple());
            grid.Children.Remove(buildingComponent.Element);
            RectangleFactory.RemoveBlinkingRectangles(grid);

            if (!_usedCells.Any())
            {
                UpdateCanCreateAndCanClearValues();
                return;
            }

            if (_usedCells.Count == 1)
            {
                _lastCoordinates = _gridBuildingComponents[0].Cell;
                ShowAvailableBuildingComponentPlaces();
                _currentBuildingComponentImage = _previousBuildingComponentImage;

                return;
            }

            _lastCoordinates = _gridBuildingComponents[_usedCells.Count - 1].Cell;
            _currentBuildingComponentImage = _previousBuildingComponentImage;
            ShowAvailableBuildingComponentPlaces();
        }

        private void BuildingComponent_Click(object sender, RoutedEventArgs e)
        {
            var componentName = (sender as Button).Name;
            _currentBuildingComponentType = (BuildingComponentType)(Enum.Parse(typeof(BuildingComponentType), componentName, true) ?? BuildingComponentType.CheckIn);
            _currentBuildingComponentImage = _buildingComponentHelper.GetBuildingComponentImage(_currentBuildingComponentType);
        }
        
        private void Run_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_fullPathBuilt)
            {
                ResetBuildingPossibilitesAfterClearOrCreate();
            }

            _gridBuildingComponents
                .Where(x => !x.Created)
                .ToList()
                .ForEach(x => x.Created = true);

            _currentBuildingComponentImage = null;

            // TODO: ValidateRunButtonVisibility();
        }

        private void ShowAvailableBuildingComponentPlaces()
        {
            var allowedRows = new List<int>();
            var allowedColumns = new List<int>();

            var (row, column) = _lastCoordinates;

            if (row == 0 && column == 0) // LEFT TOP CORNER
            {
                allowedRows.Add(row + 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

            }
            else if (column == 0 && row > 0 && row < SimulationGridOptions.GRID_MAX_ROWS) // LEFT MOST LINE
            {
                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);
            }
            else if (column == 0 && row == SimulationGridOptions.GRID_MAX_ROWS) // LEFT DOWN CORNER
            {
                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

            }
            else if (row == 0 && column > 0 && column < SimulationGridOptions.GRID_MAX_COLUMNS) // TOP MOST LINE
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);
            }

            else if (row == 0 && column == SimulationGridOptions.GRID_MAX_COLUMNS) // TOP RIGHT CORNER
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);
            }
            else if (column == SimulationGridOptions.GRID_MAX_COLUMNS && row > 0 && 
                row < SimulationGridOptions.GRID_MAX_ROWS) // RIGHT MOST LINE
            {
                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);
            }
            else if (column == SimulationGridOptions.GRID_MAX_COLUMNS && 
                row == SimulationGridOptions.GRID_MAX_ROWS) // BOTTOM RIGHT CORNER
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);
            }
            else if (row == SimulationGridOptions.GRID_MAX_ROWS && column > 0 && 
                column < SimulationGridOptions.GRID_MAX_COLUMNS) // BOTTOM MOST LINE
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);
            }
            else if (row > 0 && row < SimulationGridOptions.GRID_MAX_ROWS && column > 0 && 
                column < SimulationGridOptions.GRID_MAX_COLUMNS)
            {
                allowedRows.Add(row + 1);
                allowedColumns.Add(column);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

                allowedRows.Add(row);
                allowedColumns.Add(column - 1);
            }

            for (int i = 0; i < allowedRows.Count; i++)
            {
                var currRow = allowedRows[i];
                var currCol = allowedColumns[i];

                if (CanPlaceBlinkingRectangle(currRow, currCol))
                {
                    var blinkingRectangle = RectangleFactory.CreateBlinkingRectangle();
                    Grid.SetRow(blinkingRectangle, currRow);
                    Grid.SetColumn(blinkingRectangle, currCol);

                    SimulationGrid.Children.Add(blinkingRectangle);
                    // TODO: DisableGridNotAvailableCells(); 
                }
            }
        }

        private void DisableGridNotAvailableCells()
        {
            var gridRows = SimulationGrid.RowDefinitions;
            var gridCols = SimulationGrid.ColumnDefinitions;

            for (int i = 0; i < gridRows.Count; i++)
            {
                for (int j = 0; j < gridCols.Count; j++)
                {
                    //if (_usedCells.Contains(new KeyValuePair<int, int>(i, j)))
                    //    continue;

                    var grayRectangle = new Rectangle
                    {
                        Width = 300,
                        Height = 200,
                        IsEnabled = false,
                        Fill = new SolidColorBrush
                        {
                            Color = Colors.LightSlateGray
                        }
                    };

                    Grid.SetRow(grayRectangle, i);
                    Grid.SetColumn(grayRectangle, j);
                    SimulationGrid.Children.Add(grayRectangle);
                }
            }
        }

        private bool CanPlaceBlinkingRectangle(int row, int col)
            => _gridBuildingComponents.All(el => el.Cell.Item1 != row || el.Cell.Item2 != col);

        //private void ValidateRunButtonVisibility()
        //{
        //    var canStart = _gridNeededComponentsToRun.Contains(typeof(CheckInSettings)) &&
        //        _gridNeededComponentsToRun.Contains(typeof(ConveyorSettings)) &&
        //        _gridNeededComponentsToRun.Contains(typeof(AaSettings)) &&
        //        _gridNeededComponentsToRun.Contains(typeof(PickupAreaSettings)) &&
        //        _gridNeededComponentsToRun.Contains(typeof(PscSettings)) &&
        //        _gridNeededComponentsToRun.Contains(typeof(AscSettings));

        //    Run.GetStackPanelChildButton().IsEnabled = canStart ? true : false;
        //}

        private void ClearGridButton_Click(object sender, RoutedEventArgs e)
        {
            _previousBuildingComponentImage = null;

            ResetBuildingPossibilitesAfterClearOrCreate();
            UpdateCanCreateAndCanClearValues();

            _usedCells.Clear();
            _gridBuildingComponents.Clear();
            SimulationGrid.Children.RemoveRange(0, SimulationGrid.Children.Count);
        }

        private void UpdateCanCreateAndCanClearValues(bool canCreate = false, bool canClear = false)
        {
            SimulationGridOptions.CanCreate = canCreate;
            SimulationGridOptions.CanClear = canClear;
        }

        private void ResetBuildingPossibilitesAfterClearOrCreate()
        {
            SimulationGridOptions.CanBuildCheckIn = true;
            SimulationGridOptions.CanBuildConveyor = false;
            SimulationGridOptions.CanBuildAa = false;
            SimulationGridOptions.CanBuildAsc = false;
            SimulationGridOptions.CanBuildManyToOneConveyor = false;
            SimulationGridOptions.CanBuildPickUp = false;
            SimulationGridOptions.CanBuildPsc = false;
        }
    }
}
