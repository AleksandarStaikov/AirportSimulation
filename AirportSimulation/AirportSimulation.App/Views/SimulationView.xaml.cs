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
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    public partial class SimulationView : UserControl
    {
        private List<KeyValuePair<int, int>> _usedCells = new List<KeyValuePair<int, int>>();
        private List<GridCellElement> _gridElements = new List<GridCellElement>();
        private BitmapImage _buildingComponentImage;
        private Type _componentType;
        private List<Type> _gridNeededComponents;
        private List<Button> _buttons;
        private (int, int) _previousCoordinates;

        private LinkedList<GridCellElement> _chainedElements;

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
                Export.GetStackPanelChildButton() };
        }
        
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid grid))
                return;
            
            var (selectedRowIndex, selectedColumnIndex) = this.GetCurrentlySelectedGridCell(grid, e);
            this._previousCoordinates = (selectedRowIndex, selectedColumnIndex);
            var pair = new KeyValuePair<int, int>(selectedRowIndex, selectedColumnIndex);

            if (this.IsCellAlreadyUsed(grid, pair))
                return;
            
            if (this._buildingComponentImage == null)
                return;
            
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

            var gridCellElement = new GridCellElement
            {
                Element = rectangle,
                Cell = pair,
                SimulationType = this._componentType
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
        }

        private void BuildingComponent_Click(object sender, RoutedEventArgs e)
        {
            var componentName = (sender as Button).Name;
            var type = Enum.Parse(typeof(BuildingComponentType), componentName, true) ?? BuildingComponentType.CheckIn;

            this.CreateButton.IsEnabled = true;
            this._buttons.Where(b => b.Name != componentName).ToList().ForEach(b => b.IsEnabled = false);

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
                grid.Children.Remove(elementToRemove.Element);
                this._usedCells.Remove(pair);
                this._gridElements.Remove(elementToRemove);

                while (this._gridNeededComponents.Contains(elementToRemove.SimulationType))
                    this._gridNeededComponents.Remove(elementToRemove.SimulationType);

                if (!this._gridElements.Any())
                {
                    this._buttons.ForEach(b => b.IsEnabled = true);
                    this.CreateButton.IsEnabled = false;
                    this._buildingComponentImage = null;
                }

                this.ValidateRunButtonVisibility();

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
                this.ShowAvailableConveyorPlaces();
            }

            this._buildingComponentImage = null;
            this.ValidateRunButtonVisibility();
        }

        private void ShowAvailableConveyorPlaces()
        {
            
        }

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
    }
}
