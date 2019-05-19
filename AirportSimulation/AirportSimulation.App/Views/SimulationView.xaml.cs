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

    public partial class SimulationView : UserControl
	{
		private BuildingComponentsHelper _buildingComponentHelper = new BuildingComponentsHelper();
		private LinkedList<ConveyorBelt> _chainedBelts = new LinkedList<ConveyorBelt>();
		private ConveyorBelt _conveyorBelt = new ConveyorBelt();

		private List<(int, int)> _usedCells = new List<(int, int)>();
		private List<GridDisabledCellElement> _disabledCells = new List<GridDisabledCellElement>();
		private List<(int, int)> _blinkingRectanglesCells = new List<(int, int)>();
		private List<GridCell> _gridBuildingComponents = new List<GridCell>();

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
			_mpaBuildingComponentImage = _buildingComponentHelper.GetBuildingComponentImage(BuildingComponentType.MPA);
            
		}

		public SimulationGridOptions SimulationGridOptions { get; set; } = new SimulationGridOptions();

		private void SimulationGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is Grid grid))
			{
				return;
			}
            
			var (selectedRowIndex, selectedColumnIndex) = GridHelper.GetCurrentlySelectedGridCell(grid, e);
            _lastCoordinates = (selectedRowIndex, selectedColumnIndex);

            if (SimulationGrid.IsGridCellUsedOrDisabled((selectedRowIndex, selectedColumnIndex)))
            {
                MessageBox.Show("This cell is either used or disabled!");
                return;
            }

            var currentBuildingComponent = new SingleCellComponentFactory().CreateSingleCellComponent(_currentBuildingComponentType, "asd", (selectedRowIndex, selectedColumnIndex));
            Grid.SetColumn(currentBuildingComponent.UIElement, currentBuildingComponent.Cell.Y);
            Grid.SetRow(currentBuildingComponent.UIElement, currentBuildingComponent.Cell.X);

            grid.Children.Add(currentBuildingComponent.UIElement);
            grid.RemoveBlinkingRectangles();
            ShowAvailableBuildingComponentPlaces(currentBuildingComponent);

            //var rectangle = RectangleFactory.CreateBuildingComponentRectangle(_currentBuildingComponentImage);
            //var gridCellElement = new GridCell
            //{
            //	Id = $"X{selectedRowIndex}Y{selectedColumnIndex}",
            //	UIElement = rectangle,
            //	Cell = _lastCoordinates,
            //	ElementType = _currentBuildingComponentType,
            //	PreviousCell = _usedCells.Count > 0
            //		? _usedCells[_usedCells.Count - 1]
            //		: Tuple.Create<int?, int?>(null, null).ToValueTuple()
            //};

            //if (_gridBuildingComponents.Count > 0)
            //{
            //	_gridBuildingComponents[_usedCells.Count - 1].NextCell = gridCellElement.Cell;
            //}

            //_gridBuildingComponents.Add(gridCellElement);
            //_usedCells.Add(_lastCoordinates);
            //grid.Children.Add(rectangle);

            //Grid.SetRow(rectangle, selectedRowIndex);
            //Grid.SetColumn(rectangle, selectedColumnIndex);

            //UpdateCanCreateAndCanClearValues(true, true);
            //RectangleFactory.RemoveBlinkingRectangles(grid, _blinkingRectanglesCells);
            //ShowAvailableBuildingComponentPlaces();

            //if (_currentBuildingComponentType == BuildingComponentType.Conveyor ||
            //	_currentBuildingComponentType == BuildingComponentType.ManyToOneConveyor)
            //{
            //	_conveyorBelt.ConveyorSlots.Add(new ConveyorSlot
            //	{
            //		SlotIndex = _slotIndex++
            //	});
            //}
            //else
            //{
            //	_currentBuildingComponentImage = null;
            //	_buildingComponentHelper.DisableComponentsButtons(SimulationGridOptions);
            //}

            //if (++_hintCount < 2)
            //{
            //	MessageBox.Show("You can place a component only on the indicated places!");
            //}
        }

		private void SimulationGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
            //if (!(sender is Grid grid))
            //{
            //	return;
            //}

            //if (!_usedCells.Any())
            //{
            //	return;
            //}

            //var cell = GridHelper.GetCurrentlySelectedGridCell(grid, e).ToTuple();
            //var buildingComponent = _gridBuildingComponents.FirstOrDefault(x => x.Cell.Item1 == cell.Item1 && x.Cell.Item2 == cell.Item2);
            //(_currentBuildingComponentType, _currentBuildingComponentImage) = _buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, _step);

            //if (buildingComponent == null)
            //{
            //	MessageBox.Show("Component not found!");
            //	return;
            //}

            //if (buildingComponent.Created)
            //{
            //	MessageBox.Show("You cannot remove an already created component!");
            //	return;
            //}

            //if (_gridBuildingComponents.Count > 1 && !buildingComponent.PreviousCell.IsCellNull() && !buildingComponent.NextCell.IsCellNull())
            //{
            //	MessageBox.Show("You cannot detach a component between other components!");
            //	return;
            //}

            //DetachLinkedCells(buildingComponent);
            //_gridBuildingComponents.Remove(buildingComponent);
            //_usedCells.Remove(cell.ToValueTuple());
            //grid.Children.Remove(buildingComponent.UIElement);
            //RectangleFactory.RemoveBlinkingRectangles(grid, _blinkingRectanglesCells);

            //if (!_usedCells.Any())
            //{
            //	UpdateCanCreateAndCanClearValues();
            //	ClearGridButton_Click(sender, e);
            //	return;
            //}

            //if (_usedCells.Count == 1)
            //{
            //	_lastCoordinates = _gridBuildingComponents[0].Cell;
            //	ShowAvailableBuildingComponentPlaces();
            //	return;
            //}

            //_lastCoordinates = _gridBuildingComponents[_usedCells.Count - 1].Cell;
            //ShowAvailableBuildingComponentPlaces();
        }

        private void DetachLinkedCells(GridCell cell)
		{
			//var previousCell =
			//	_gridBuildingComponents.FirstOrDefault(
			//		x => x.Cell.Item1 == cell.PreviousCell.Item1 && x.Cell.Item2 == cell.PreviousCell.Item2);

			//if (previousCell != null)
			//{
			//	previousCell.NextCell = _nullTuple;
			//}

			//var nextCell =
			//	_gridBuildingComponents.FirstOrDefault(
			//		x => x.Cell.Item1 == cell.NextCell.Item1 && x.Cell.Item2 == cell.NextCell.Item2);

			//if (nextCell != null)
			//{
			//	nextCell.PreviousCell = _nullTuple;
			//}
		}

		private void BuildingComponent_Click(object sender, RoutedEventArgs e)
		{
			var componentName = (sender as Button)?.Name;

			if (componentName == null)
				return;

			_currentBuildingComponentType = (BuildingComponentType) Enum.Parse(typeof(BuildingComponentType), componentName, true);
			_currentBuildingComponentImage = _buildingComponentHelper.GetBuildingComponentImage(_currentBuildingComponentType);
		}

		private void Run_Click(object sender, RoutedEventArgs e)
		{
			var engine = ContainerConfig.Resolve<IEngine>();
			var simulationSettings = new SimulationSettings();

			foreach (var belt in _chainedBelts)
			{
				var conveyorSettings = new ConveyorSettings { Length = belt.ConveyorSlots.Count };
				simulationSettings.ConveyorSettingsMpaToAa.Add(conveyorSettings);
			}

			engine.RunDemo(simulationSettings);
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			//_gridBuildingComponents
			//	.Where(x => !x.Created)
			//	.ToList()
			//	.ForEach(x => x.Created = true);

			//_fullPathBuilt = _step == 7;

			//if (_fullPathBuilt)
			//{
			//	ResetBuildingPossibilitesAfterClearOrCreate();
			//}
			//else
			//{
			//	(_currentBuildingComponentType, _currentBuildingComponentImage) =
			//		_buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, ++_step);
			//}

			//if (_conveyorBelt.ConveyorSlots.Any())
			//{
			//	_chainedBelts.AddLast(_conveyorBelt);
			//	_slotIndex = 0;
			//	_conveyorBelt = new ConveyorBelt();
			//}

			// TODO: ValidateRunButtonVisibility();
			}

		private void ShowAvailableBuildingComponentPlaces(SingleCellBuildingComponent component)
		{
            SimulationGrid.RemoveDisabledGridCellsIfPossible();

            foreach (var blinkingRec in component.PossibleNeighbours)
            {
                if (SimulationGrid.CanPlaceBlinkingRectangle(blinkingRec.Cell))
                {
                    Grid.SetRow(blinkingRec.UIElement, blinkingRec.Cell.X);
                    Grid.SetColumn(blinkingRec.UIElement, blinkingRec.Cell.Y);

                    SimulationGrid.Children.Add(blinkingRec.UIElement);
                }
            }

            DisableRestOfTheGrid();
        }

        private void DisableRestOfTheGrid()
		{
			for (var i = 0; i < SimulationGrid.RowDefinitions.Count; i++)
			{
				for (var j = 0; j < SimulationGrid.ColumnDefinitions.Count; j++)
				{
					if (!SimulationGrid.CanDisable((i, j)))
						continue;

					var disabledRectangle = RectangleFactory.CreateDisabledRectangle(200, 200);
					Grid.SetRow(disabledRectangle, i);
					Grid.SetColumn(disabledRectangle, j);
					SimulationGrid.Children.Add(disabledRectangle);
				}
			}
		}

		private bool CanPlaceBlinkingRectangle(int row, int col)
			=> _gridBuildingComponents.All(el => el.Cell.X != row || el.Cell.Y != col);

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
			(_currentBuildingComponentType, _currentBuildingComponentImage) = _buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, 1);
			ResetBuildingPossibilitesAfterClearOrCreate();
			UpdateCanCreateAndCanClearValues();

			_usedCells.Clear();
			_disabledCells.Clear();
			_gridBuildingComponents.Clear();

			_step = 1;
			//RectangleFactory.RemoveBlinkingRectangles(SimulationGrid, _blinkingRectanglesCells);
			SimulationGrid.Children.RemoveRange(0, SimulationGrid.Children.Count);
		}

		private void UpdateCanCreateAndCanClearValues(bool canCreate = false, bool canClear = false)
		{
			SimulationGridOptions.CanCreate = canCreate;
			SimulationGridOptions.CanClear = canClear;
		}

		private void ResetBuildingPossibilitesAfterClearOrCreate()
		{
			foreach (var disabledCell in _disabledCells)
			{
				SimulationGrid.Children.Remove(disabledCell.DisabledElement);
			}

			_step = 1;

			_disabledCells.Clear();
			//RectangleFactory.RemoveBlinkingRectangles(SimulationGrid, _blinkingRectanglesCells);
			UpdateCanCreateAndCanClearValues();
			(_currentBuildingComponentType, _currentBuildingComponentImage) =_buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, 1);
		}
	}
}
