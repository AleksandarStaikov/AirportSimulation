namespace AirportSimulation.App.Views
{
    using Common;
    using Common.Models;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using AirportSimulation.App.Models;
    using AirportSimulation.App.Helpers;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;

    public partial class SimulationView : UserControl
    {
        private Action<BuildingComponentType> buildingComponentClicked = delegate { };
        private BlinkingCellsPainter cellsPainter;
        private BitmapImage _currentBuildingComponentImage;
        private BuildingComponentType _currentBuildingComponentType = BuildingComponentType.CheckIn;

        public SimulationView()
        {
            InitializeComponent();

            //(_currentBuildingComponentType, _currentBuildingComponentImage) =
            //    _buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, _step,
            //        true);

            //_currentBuildingComponentImage = _buildingComponentHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn);
            cellsPainter = new BlinkingCellsPainter(SimulationGrid);
            InitializeClickableGridCells();
        }

        public SimulationGridOptions SimulationGridOptions { get; set; } = new SimulationGridOptions();

        private void InitializeClickableGridCells()
        {
            SimulationGrid.Children.Clear();

            for (var i = 0; i < SimulationGrid.RowDefinitions.Count; i++)
            {
                for (var j = 0; j < SimulationGrid.ColumnDefinitions.Count; j++)
                {
                    var enabledRectangle = new MutantRectangle((i, j));

                    enabledRectangle.MouseDown += EnabledRectangle_MouseDown;
                    buildingComponentClicked += enabledRectangle.On_BuildingComponentClicked;
                    enabledRectangle.ReadyToGoNext += EnableNextButton;
                    Grid.SetColumn(enabledRectangle, j);
                    Grid.SetRow(enabledRectangle, i);

                    SimulationGrid.Children.Add(enabledRectangle);
                }
            }
        }

        private void EnabledRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MutantRectangle clickedRectangle = sender as MutantRectangle;

            clickedRectangle.On_Click(_currentBuildingComponentType);
        }

        private void EnableNextButton() => SimulationGridOptions.CanNext = true;


        private void SimulationGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SimulationGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BuildingComponent_Click(object sender, RoutedEventArgs e)
        {
            var componentName = (sender as Button)?.Name;

            if (componentName == null)
                return;

            _currentBuildingComponentType = (BuildingComponentType)Enum.Parse(typeof(BuildingComponentType), componentName, true);
            _currentBuildingComponentImage = BuildingComponentsHelper.GetBuildingComponentImage(_currentBuildingComponentType);

            buildingComponentClicked(_currentBuildingComponentType);
        }

        private void ClearGridButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Clear Grid
            InitializeClickableGridCells();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = ConvertToSettingsService.Serialize();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented
                };


                using (var fs = new FileStream("../../myTest.txt", FileMode.Create, FileAccess.Write))
                {
                    try
                    {
                        var bf = new BinaryFormatter();
                        bf.Serialize(fs, data);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (StackOverflowException ex)
            {
                var a = ex;
                // ignored
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            using (var fs = new FileStream("../../myTest.txt", FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var bf = new BinaryFormatter();
                    var res = bf.Deserialize(fs);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
