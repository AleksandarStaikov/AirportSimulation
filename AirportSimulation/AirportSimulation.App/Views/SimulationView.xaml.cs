namespace AirportSimulation.App.Views
{
    using AirportSimulation.App.Resources;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using AirportSimulation.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    public partial class SimulationView : UserControl
    {
        private List<KeyValuePair<int, int>> _usedCells = new List<KeyValuePair<int, int>>();
        private List<GridCellElement> _gridElements = new List<GridCellElement>();
        private LinkedList<GridCellElement> _chainedElements;
        private BitmapImage _buildingComponentImage;
        private Type _componentType;
        private List<Type> _gridNeededComponents;
        private List<Button> _buttons;
        private (int, int) _previousCoordinates;
        private int GRID_MAX_ROWS = 14;
        private int GRID_MAX_COLUMNS = 19;
        private bool _created = false;

        public string GridRows => $"*#{++GRID_MAX_ROWS}";

        public string GridColumns => $"*#{++GRID_MAX_COLUMNS}";


        public SimulationView()
        {
            InitializeComponent();
            this._gridNeededComponents = new List<Type>();
            this._chainedElements = new LinkedList<GridCellElement>();
            this._buttons = new List<Button> {
                Conveyor,
                CheckIn,
                AA,
                PSC,
                ASC,
                PA,
                Import.GetStackPanelChildButton(),
                Export.GetStackPanelChildButton()
            };
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid grid))
            {
                return;
            }

            var (selectedRowIndex, selectedColumnIndex) = this.GetCurrentlySelectedGridCell(grid, e);
            this._previousCoordinates = (selectedRowIndex, selectedColumnIndex);
            var pair = new KeyValuePair<int, int>(selectedRowIndex, selectedColumnIndex);

            if (this.IsCellAlreadyUsed(grid, pair))
            {
                return;
            }

            if (this._buildingComponentImage == null)
            {
                return;
            }

            var rectangle = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = new ImageBrush
                {
                    Stretch = Stretch.Fill,
                    ImageSource = this._buildingComponentImage
                }
            };

            Grid.SetZIndex(rectangle, 1000);

            var gridCellElement = new GridCellElement
            {
                Element = rectangle,
                Cell = pair,
                SimulationType = this._componentType,
            };

            this._gridElements.Add(gridCellElement);
            this._gridNeededComponents.Add(this._componentType);

            grid.Children.Add(rectangle);
            this._usedCells.Add(pair);

            Grid.SetRow(rectangle, selectedRowIndex);
            Grid.SetColumn(rectangle, selectedColumnIndex);

            if (this._gridElements.Count == 1)
            {
                this._chainedElements.AddFirst(gridCellElement);
                this._buildingComponentImage = null;
            }
            else
            {
                this._chainedElements.AddLast(gridCellElement);
            }

            this.RemoveBlinkingRectangles();
            this.ShowAvailableComponentPlaces();
            this.ValidateClearButtonVisibility();
        }

        private void BuildingComponent_Click(object sender, RoutedEventArgs e)
        {
            var componentName = (sender as Button).Name;
            var type = Enum.Parse(typeof(BuildingComponentType), componentName, true) ?? BuildingComponentType.CheckIn;

            this._created = false;
            this.CreateButton.IsEnabled = true;
            this._buttons
                .Where(b => b.Name != componentName)
                .ToList()
                .ForEach(b => b.IsEnabled = false);

            switch (type)
            {
                case BuildingComponentType.CheckIn:
                    this._buildingComponentImage = GetComponentImage("Resources/check-in.png");
                    this._componentType = typeof(CheckInSettings);
                    this.CheckIn.IsEnabled = false; // TODO: Remove later, for now we need only 1 check-in
                    break;

                case BuildingComponentType.Conveyor:
                    this._buildingComponentImage = GetComponentImage("Resources/conveyor.png");
                    this._componentType = typeof(ConveyorSettings);
                    break;

                case BuildingComponentType.PA:
                    this._buildingComponentImage = GetComponentImage("Resources/PickUpBaggage.png");
                    this._componentType = typeof(PickupAreaSettings);
                    break;

                case BuildingComponentType.PSC:
                    this._buildingComponentImage = GetComponentImage("Resources/PSCbaggage.png");
                    this._componentType = typeof(PscSettings);
                    break;

                case BuildingComponentType.ASC:
                    this._buildingComponentImage = GetComponentImage("Resources/AdvancedCheckBaggage.png");
                    this._componentType = typeof(AscSettings);
                    break;

                case BuildingComponentType.AA:
                    this._buildingComponentImage = GetComponentImage("Resources/airplane-shape.png");
                    this._componentType = typeof(AaSettings);
                    break;

                default:
                    break;
            }
        }

        private BitmapImage GetComponentImage(string fileLocation)
            => new BitmapImage(new Uri($"../../{fileLocation}", UriKind.Relative));

        private bool IsCellAlreadyUsed(Grid grid, KeyValuePair<int, int> pair)
        {
            if (this._usedCells.Contains(pair))
            {
                var elementToRemove = this._gridElements.FirstOrDefault(el => el.Cell.Key == pair.Key && el.Cell.Value == pair.Value);
                if (!elementToRemove.Created)
                {
                    if (this._gridElements.Any())
                    {
                        var index = this._gridElements.IndexOf(elementToRemove) != -1 
                            ? this._gridElements.IndexOf(elementToRemove) 
                            : 0;
                        var prevCoordinates = this._gridElements[index == 0 ? 0 : index - 1].Cell;
                        this._previousCoordinates = (prevCoordinates.Key, prevCoordinates.Value);
                    }

                    grid.Children.Remove(elementToRemove.Element);
                    this._usedCells.Remove(pair);
                    this._gridElements.Remove(elementToRemove);

                    while (this._gridNeededComponents.Contains(elementToRemove.SimulationType))
                        this._gridNeededComponents.Remove(elementToRemove.SimulationType);

                    this.RemoveBlinkingRectangles();
                    this.ShowAvailableComponentPlaces();
                }

                if (!this._gridElements.Any())
                {
                    this.CreateButton.IsEnabled = false;
                    this.CheckIn.IsEnabled = true;
                    this.RemoveBlinkingRectangles();
                }

                this.ValidateRunButtonVisibility();
                this.ValidateClearButtonVisibility();

                return true;
            }

            return false;
        }

        private (int, int) GetCurrentlySelectedGridCell(Grid grid, MouseButtonEventArgs e)
        {
            var selectedColumnIndex = -1;
            var selectedRowIndex = -1;

            var pos = e.GetPosition(grid);
            var temp = pos.X;

            for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var colDef = grid.ColumnDefinitions[i];
                temp -= colDef.ActualWidth;

                if (temp <= -1)
                {
                    selectedColumnIndex = i;
                    break;
                }
            }

            temp = pos.Y;

            for (var i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var rowDef = grid.RowDefinitions[i];
                temp -= rowDef.ActualHeight;

                if (temp <= -1)
                {
                    selectedRowIndex = i;
                    break;
                }
            }

            return (selectedRowIndex, selectedColumnIndex);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //this._buttons.ForEach(b => b.IsEnabled = true);

            if (this._gridElements.Count == 1 && this._previousCoordinates.ToTuple() != null)
            {
                this.Conveyor.IsEnabled = true;
            }
            else
            {
                if (this._gridElements.All(el => el.SimulationType != typeof(PscSettings)))
                {
                    this.Conveyor.IsEnabled = false;
                    this.PSC.IsEnabled = true;
                }
                else
                {
                    this.Conveyor.IsEnabled = true;
                    this.PSC.IsEnabled = false;
                }
            }

            this._gridElements.ForEach(el => el.Created = true);
            this.ShowAvailableComponentPlaces();
            this._buildingComponentImage = null;
            this.ValidateRunButtonVisibility();
        }

        private void RemoveBlinkingRectangles()
        {
            var rectanglesToRemove = this.SimulationGrid
                    .Children
                    .OfType<Rectangle>()
                    .Where(r => r.Uid == Constants.BLINKING_RECTANGLE_UID)
                    .ToList();

            foreach (var rec in rectanglesToRemove)
            {
                this.SimulationGrid.Children.Remove(rec);
            }
        }

        private void ShowAvailableComponentPlaces()
        {
            var allowedRows = new List<int>();
            var allowedColumns = new List<int>();

            var (row, column) = this._previousCoordinates;

            if (row == 0 && column == 0) // LEFT TOP CORNER
            {
                allowedRows.Add(row + 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

            }
            else if (column == 0 && row > 0 && row < GRID_MAX_ROWS) // LEFT MOST LINE
            {
                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);
            }
            else if (column == 0 && row == GRID_MAX_ROWS) // LEFT DOWN CORNER
            {
                allowedRows.Add(row);
                allowedColumns.Add(column + 1);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

            }
            else if (row == 0 && column > 0 && column < GRID_MAX_COLUMNS) // TOP MOST LINE
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);
            }

            else if (row == 0 && column == GRID_MAX_COLUMNS) // TOP RIGHT CORNER
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);
            }
            else if (column == GRID_MAX_COLUMNS && row > 0 && row < GRID_MAX_ROWS) // RIGHT MOST LINE
            {
                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row + 1);
                allowedColumns.Add(column);
            }
            else if (column == GRID_MAX_COLUMNS && row == GRID_MAX_ROWS) // BOTTOM RIGHT CORNER
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);
            }
            else if (row == GRID_MAX_ROWS && column > 0 && column < GRID_MAX_COLUMNS) // BOTTOM MOST LINE
            {
                allowedRows.Add(row);
                allowedColumns.Add(column - 1);

                allowedRows.Add(row - 1);
                allowedColumns.Add(column);

                allowedRows.Add(row);
                allowedColumns.Add(column + 1);
            }
            else if (row > 0 && row < GRID_MAX_ROWS && column > 0 && column < GRID_MAX_COLUMNS)
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
                    var rec = GetBlinkingRectangle();
                    Grid.SetRow(rec, currRow);
                    Grid.SetColumn(rec, currCol);

                    var pair = new KeyValuePair<int, int>(currRow, currCol);
                    //this._usedCells.Add(pair);
                    //this._gridElements.Add(new GridCellElement
                    //{
                    //    Cell = pair,
                    //    Element = rec
                    //});

                    this.SimulationGrid.Children.Add(rec);
                    //this.DisableGridNotAvailableCells();
                }

            }
        }

        private void DisableGridNotAvailableCells()
        {
            var gridRows = this.SimulationGrid.RowDefinitions;
            var gridCols = this.SimulationGrid.ColumnDefinitions;

            for (int i = 0; i < gridRows.Count; i++)
            {
                for (int j = 0; j < gridCols.Count; j++)
                {
                    if (this._usedCells.Contains(new KeyValuePair<int, int>(i, j)))
                        continue;

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
                    this.SimulationGrid.Children.Add(grayRectangle);
                }
            }
        }

        private bool CanPlaceBlinkingRectangle(int row, int col)
            => this._gridElements.All(el => el.Cell.Key != row || el.Cell.Value != col);

        private void ValidateRunButtonVisibility()
        {
            var canStart = this._gridNeededComponents.Contains(typeof(CheckInSettings)) &&
                this._gridNeededComponents.Contains(typeof(ConveyorSettings)) &&
                this._gridNeededComponents.Contains(typeof(AaSettings)) &&
                this._gridNeededComponents.Contains(typeof(PickupAreaSettings)) &&
                this._gridNeededComponents.Contains(typeof(PscSettings)) &&
                this._gridNeededComponents.Contains(typeof(AscSettings));

            this.Run.GetStackPanelChildButton().IsEnabled = canStart ? true : false;
        }

        private void ValidateClearButtonVisibility() =>
            this.ClearGridButton.IsEnabled = this._gridElements.Any() ? true : false;

        private void ClearGridButton_Click(object sender, RoutedEventArgs e)
        {
            this._gridElements.Clear();
            this._usedCells.Clear();
            this.SimulationGrid.Children.RemoveRange(0, this.SimulationGrid.Children.Count);
            this.CreateButton.IsEnabled = false;
            this.CheckIn.IsEnabled = true;
            this.Conveyor.IsEnabled = false;
            this._buildingComponentImage = null;
            this.ValidateClearButtonVisibility();
        }

        private Rectangle GetBlinkingRectangle()
        {
            var blinkingRec = new Rectangle
            {
                Width = 300,
                Height = 200,
                Opacity = 0.5,
                Fill = new SolidColorBrush(Colors.White),
                Uid = "BlinkingRectangle"
            };

            var animation = new ColorAnimation
            {
                From = Colors.White,
                To = Colors.ForestGreen,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };

            blinkingRec.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            return blinkingRec;
        }
    }
}
