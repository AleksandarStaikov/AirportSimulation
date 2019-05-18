namespace AirportSimulation.App.Views
{
	using Helpers;
	using Resources;
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
	using Utility;

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
		private BuildingComponentType _currentBuildingComponentType;

		private (int, int) _lastCoordinates;
		private static int _hintCount;
		private static int _slotIndex;
		private static int _step = 1;

		private bool _fullPathBuilt = false;
		private readonly (int?, int?) _nullTuple = Tuple.Create<int?, int?>(null, null).ToValueTuple();

		public SimulationView()
		{
			InitializeComponent();

			(_currentBuildingComponentType, _currentBuildingComponentImage) =
				_buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, _step,
					true);

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


			if (GridHelper.IsCellDisabled(_disabledCells.Select(x => x.DisabledCell).ToList(), _lastCoordinates))
			{
				MessageBox.Show("This cell is disabled!");
				return;
			}

			if (GridHelper.IsCellAlreadyUsed(_usedCells, _lastCoordinates))
			{
				MessageBox.Show("This cell is already used. Right click to clear it.");
				return;
			}

			var rectangle = RectangleFactory.CreateBuildingComponentRectangle(_currentBuildingComponentImage);
			var gridCellElement = new GridCell
			{
				Id = $"X{selectedRowIndex}Y{selectedColumnIndex}",
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
			RectangleFactory.RemoveBlinkingRectangles(grid, _blinkingRectanglesCells);
			ShowAvailableBuildingComponentPlaces();

			if (_currentBuildingComponentType == BuildingComponentType.Conveyor ||
				_currentBuildingComponentType == BuildingComponentType.ManyToOneConveyor)
			{
				_conveyorBelt.ConveyorSlots.Add(new ConveyorSlot
				{
					SlotIndex = _slotIndex++
				});
			}
			else
			{
				_currentBuildingComponentImage = null;
				_buildingComponentHelper.DisableComponentsButtons(SimulationGridOptions);
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
			(_currentBuildingComponentType, _currentBuildingComponentImage) = _buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, _step);

			if (buildingComponent == null)
			{
				MessageBox.Show("Component not found!");
				return;
			}

			if (buildingComponent.Created)
			{
				MessageBox.Show("You cannot remove an already created component!");
				return;
			}

			if (_gridBuildingComponents.Count > 1 && !buildingComponent.PreviousCell.IsCellNull() && !buildingComponent.NextCell.IsCellNull())
			{
				MessageBox.Show("You cannot detach a component between other components!");
				return;
			}

			DetachLinkedCells(buildingComponent);
			_gridBuildingComponents.Remove(buildingComponent);
			_usedCells.Remove(cell.ToValueTuple());
			grid.Children.Remove(buildingComponent.Element);
			RectangleFactory.RemoveBlinkingRectangles(grid, _blinkingRectanglesCells);

			if (!_usedCells.Any())
			{
				UpdateCanCreateAndCanClearValues();
				ClearGridButton_Click(sender, e);
				return;
			}

			if (_usedCells.Count == 1)
			{
				_lastCoordinates = _gridBuildingComponents[0].Cell;
				ShowAvailableBuildingComponentPlaces();
				return;
			}

			_lastCoordinates = _gridBuildingComponents[_usedCells.Count - 1].Cell;
			ShowAvailableBuildingComponentPlaces();
		}

		private void DetachLinkedCells(GridCell cell)
		{
			var previousCell =
				_gridBuildingComponents.FirstOrDefault(
					x => x.Cell.Item1 == cell.PreviousCell.Item1 && x.Cell.Item2 == cell.PreviousCell.Item2);

			if (previousCell != null)
			{
				previousCell.NextCell = _nullTuple;
			}

			var nextCell =
				_gridBuildingComponents.FirstOrDefault(
					x => x.Cell.Item1 == cell.NextCell.Item1 && x.Cell.Item2 == cell.NextCell.Item2);

			if (nextCell != null)
			{
				nextCell.PreviousCell = _nullTuple;
			}
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
			_gridBuildingComponents
				.Where(x => !x.Created)
				.ToList()
				.ForEach(x => x.Created = true);

			_fullPathBuilt = _step == 7;

			if (_fullPathBuilt)
			{
				ResetBuildingPossibilitesAfterClearOrCreate();
			}
			else
			{
				(_currentBuildingComponentType, _currentBuildingComponentImage) =
					_buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, ++_step);
			}

			if (_conveyorBelt.ConveyorSlots.Any())
			{
				_chainedBelts.AddLast(_conveyorBelt);
				_slotIndex = 0;
				_conveyorBelt = new ConveyorBelt();
			}

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

				if (!CanPlaceBlinkingRectangle(currRow, currCol))
					continue;

				var blinkingRectangle = RectangleFactory.CreateBlinkingRectangle();

				Grid.SetRow(blinkingRectangle, currRow);
				Grid.SetColumn(blinkingRectangle, currCol);

				_blinkingRectanglesCells.Add((currRow, currCol));
				SimulationGrid.Children.Add(blinkingRectangle);
			}

			EnableGridAvailableCells();
		}

		private void EnableGridAvailableCells()
		{
			for (var i = 0; i < _usedCells.Count; i++)
			{
				RemoveDisabledRectangleIfPossible((_usedCells[i].Item1, _usedCells[i].Item2));
			}

			for (var i = 0; i < _blinkingRectanglesCells.Count; i++)
			{
				RemoveDisabledRectangleIfPossible((_blinkingRectanglesCells[i].Item1, _blinkingRectanglesCells[i].Item2));
			}

			DisableRestOfTheGrid();
		}

		private void RemoveDisabledRectangleIfPossible((int, int) coordinates)
		{
			var disabledCell = _disabledCells
				.FirstOrDefault(x => x.DisabledCell.Item1 == coordinates.Item1 && x.DisabledCell.Item2 == coordinates.Item2);

			if (disabledCell == null)
				return;

			_disabledCells.Remove(disabledCell);
			SimulationGrid.Children.Remove(disabledCell.DisabledElement);
		}

		private void DisableRestOfTheGrid()
		{
			for (var i = 0; i < SimulationGrid.RowDefinitions.Count; i++)
			{
				for (var j = 0; j < SimulationGrid.ColumnDefinitions.Count; j++)
				{
					var disabledCell =
						_disabledCells.FirstOrDefault(x => x.DisabledCell.Item1 == i && x.DisabledCell.Item2 == j);

					if (_usedCells.Contains((i, j)) || 
						_blinkingRectanglesCells.Contains((i, j)) ||
						_disabledCells.Contains(disabledCell))
						continue;

					var disabledRectangle = RectangleFactory.CreateDisabledRectangle(200, 200);
					_disabledCells.Add(new GridDisabledCellElement
					{
						DisabledCell = (i, j),
						DisabledElement = disabledRectangle
					});

					Grid.SetRow(disabledRectangle, i);
					Grid.SetColumn(disabledRectangle, j);
					SimulationGrid.Children.Add(disabledRectangle);
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
			(_currentBuildingComponentType, _currentBuildingComponentImage) = _buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, 1);
			ResetBuildingPossibilitesAfterClearOrCreate();
			UpdateCanCreateAndCanClearValues();

			_usedCells.Clear();
			_disabledCells.Clear();
			_gridBuildingComponents.Clear();

			_step = 1;
			RectangleFactory.RemoveBlinkingRectangles(SimulationGrid, _blinkingRectanglesCells);
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
			RectangleFactory.RemoveBlinkingRectangles(SimulationGrid, _blinkingRectanglesCells);
			UpdateCanCreateAndCanClearValues();
			(_currentBuildingComponentType, _currentBuildingComponentImage) =_buildingComponentHelper.EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions, 1);
		}
	}
}
