namespace AirportSimulation.App.Views
{
    using AirportSimulation.App.Resources;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public SimulationView()
        {
            InitializeComponent();
            this._buildingComponentImage = GetComponentImage("Resources/check-in.png");
            this._componentType = typeof(CheckInSettings);
        }
        
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid grid))
                return;

            var (selectedRowIndex, selectedColumnIndex) = this.GetCurrentlySelectedGridCell(grid, e);
            var pair = new KeyValuePair<int, int>(selectedRowIndex, selectedColumnIndex);

            if (this.IsCellAlreadyUsed(grid, pair))
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

            this._gridElements.Add(new GridCellElement
            {
                Element = rectangle,
                Cell = pair,
                SimulationType = this._componentType
            });

            grid.Children.Add(rectangle);
            this._usedCells.Add(pair);

            Grid.SetRow(rectangle, selectedRowIndex);
            Grid.SetColumn(rectangle, selectedColumnIndex);

            if (this._componentType == typeof(CheckInSettings))
            {

            }
        }

        private void BuildingComponent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var componentName = (sender as Button).Name;
            var type = Enum.Parse(typeof(BuildingComponentType), componentName, true) ?? BuildingComponentType.CheckIn;

            switch (type)
            {
                case BuildingComponentType.CheckIn:
                    this._buildingComponentImage = GetComponentImage("Resources/check-in.png");
                    this._componentType = typeof(CheckInSettings);
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

        private void Run_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
    }
}
