namespace AirportSimulation.App.Views
{
	using Infrastructure;
	using Common;
	using Common.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media.Imaging;
	using Core;
	using Core.Contracts;
    using AirportSimulation.App.Models;
    using AirportSimulation.App.Helpers;
    using CuttingEdge.Conditions;
    using AirportSimulation.Utility;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public partial class SimulationView : UserControl
	{
        private Action<BuildingComponentType> buildingComponentClicked = delegate { };
        private BlinkingCellsPainter cellsPainter;
		//private BuildingComponentsHelper _buildingComponentHelper = new BuildingComponentsHelper();
		private LinkedList<ConveyorBelt> _chainedBelts = new LinkedList<ConveyorBelt>();
		private ConveyorBelt _conveyorBelt = new ConveyorBelt();

		private List<(int, int)> _usedCells = new List<(int, int)>();
		private List<GridDisabledCellElement> _disabledCells = new List<GridDisabledCellElement>();
		private List<(int, int)> _blinkingRectanglesCells = new List<(int, int)>();
		private List<BlinkingCell> _gridBuildingComponents = new List<BlinkingCell>();

		private BitmapImage _currentBuildingComponentImage;
		private BitmapImage _previousBuildingComponentImage;
		private BitmapImage _mpaBuildingComponentImage;
        private BuildingComponentType _currentBuildingComponentType = BuildingComponentType.CheckIn;

		private (int, int) _lastCoordinates;
		private static int _hintCount;
		private static int _slotIndex;
		private static int _step = 1;

		private bool _fullPathBuilt = false;
		private readonly (int?, int?) _nullTuple = Tuple.Create<int?, int?>(null, null).ToValueTuple();

		public SimulationView()
		{
			InitializeComponent();

            //(_currentBuildingComponentType, _currentBuildingComponentImage) =
            //    _buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, _step,
            //        true);

            //_currentBuildingComponentImage = _buildingComponentHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn);
			_mpaBuildingComponentImage = BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.MPA);
            cellsPainter = new BlinkingCellsPainter(SimulationGrid);
            InitializeClickableGridCells();
		}

		public SimulationGridOptions SimulationGridOptions { get; set; } = new SimulationGridOptions();

        private void InitializeClickableGridCells()
        {
            for (var i = 0; i < SimulationGrid.RowDefinitions.Count; i++)
            {
                for (var j = 0; j < SimulationGrid.ColumnDefinitions.Count; j++)
                {
                    var enabledRectangle = new MutantRectangle((i, j));

                    enabledRectangle.MouseDown += EnabledRectangle_MouseDown;
                    buildingComponentClicked += enabledRectangle.On_BuildingComponentClicked;
                    enabledRectangle.ReadyToRun += EnableRunButton;
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

        private void EnableRunButton()
        {
            SimulationGridOptions.CanRun = !SimulationGridOptions.CanRun;
        }

        private void SimulationGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            
        }

		private void SimulationGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
            
        }

        private void DetachLinkedCells(BlinkingCell cell)
		{

		}

		private void BuildingComponent_Click(object sender, RoutedEventArgs e)
		{
			var componentName = (sender as Button)?.Name;

			if (componentName == null)
				return;

			_currentBuildingComponentType = (BuildingComponentType) Enum.Parse(typeof(BuildingComponentType), componentName, true);
			_currentBuildingComponentImage = BuildingComponentsHelper.GetBuildingComponentImage(_currentBuildingComponentType);

            buildingComponentClicked(_currentBuildingComponentType);
		}

		private void Run_Click(object sender, RoutedEventArgs e)
		{
            var data = ConvertToSettingsService.Convert();
        }



        private void ClearGridButton_Click(object sender, RoutedEventArgs e)
		{
			//TODO: Clear Grid
		}
	}
}
