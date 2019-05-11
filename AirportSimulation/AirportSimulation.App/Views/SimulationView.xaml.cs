namespace AirportSimulation.App.Views
{
    using AirportSimulation.App.Resources;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using AirportSimulation.Utility;
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
        private Type _simulationType;

        private List<Button> _buttons;
             
        public SimulationView()
        {
            InitializeComponent();
            this._buttons = new List<Button> {
                Conveyor,
                CheckIn,
                AA,
                PSC,
                ASC,
                PA,
                Run.GetStackPanelChildButton(),
                Import.GetStackPanelChildButton(),
                Export.GetStackPanelChildButton() };
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedColumnIndex = -1;
            var selectedRowIndex = -1;
            var grid = sender as Grid;

            if (grid != null)
            {
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
            }

            var pair = new KeyValuePair<int, int>(selectedRowIndex, selectedColumnIndex);

            if (this._usedCells.Contains(pair))
            {
                var elementToRemove = _gridElements.FirstOrDefault(el => el.Cell.Key == pair.Key && el.Cell.Value == pair.Value);
                grid.Children.Remove(elementToRemove.Element);
                this._usedCells.Remove(pair);
                _gridElements.Remove(elementToRemove);

                if (!this._gridElements.Any())
                {
                    this._buttons.ForEach(b => b.IsEnabled = true);
                    this.CreateButton.IsEnabled = false;
                }

                return;
            }

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

            _gridElements.Add(new GridCellElement
            {
                Element = rectangle,
                Cell = pair,
                SimulationType = this._simulationType
            });

            grid.Children.Add(rectangle);
            this._usedCells.Add(pair);

            Grid.SetRow(rectangle, selectedRowIndex);
            Grid.SetColumn(rectangle, selectedColumnIndex);
        }

        private void BuildingComponent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var componentName = (sender as Button).Name;
            var type = Enum.Parse(typeof(BuildingComponentType), componentName, true) ?? BuildingComponentType.CheckIn;

            this.CreateButton.IsEnabled = true;
            this._buttons.Where(b => b.Name != componentName).ToList().ForEach(b => b.IsEnabled = false);

            switch (type)
            {
                case BuildingComponentType.CheckIn:
                    this._buildingComponentImage = GetComponentImage("Resources/check-in.png");
                    this._simulationType = typeof(CheckInSettings);
                    break;

                case BuildingComponentType.Conveyor:
                    this._buildingComponentImage = GetComponentImage("Resources/conveyor.png");
                    this._simulationType = typeof(ConveyorSettings);
                    break;

                case BuildingComponentType.PA:
                    this._buildingComponentImage = GetComponentImage("Resources/PickUpBaggage.png");
                    this._simulationType = typeof(PickupAreaSettings);
                    break;

                case BuildingComponentType.PSC:
                    this._buildingComponentImage = GetComponentImage("Resources/PSCbaggage.png");
                    this._simulationType = typeof(PscSettings);
                    break;

                case BuildingComponentType.ASC:
                    this._buildingComponentImage = GetComponentImage("Resources/AdvancedCheckBaggage.png");
                    this._simulationType = typeof(AscSettings);
                    break;

                case BuildingComponentType.AA:
                    this._buildingComponentImage = GetComponentImage("Resources/airplane-shape.png");
                    this._simulationType = typeof(AaSettings);
                    break;

                default:
                    break;
            }
            
        }

        private BitmapImage GetComponentImage(string fileLocation)
            => new BitmapImage(new Uri($"../../{fileLocation}", UriKind.Relative));

        private void Run_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this._buttons.ForEach(b => b.IsEnabled = true);
            this._buildingComponentImage = null;
        }
    }
}
