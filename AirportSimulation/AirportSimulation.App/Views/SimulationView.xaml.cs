namespace AirportSimulation.App.Views
{
	using Common;
	using Common.Models;
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media.Imaging;
	using Models;
	using Helpers;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Windows.Media;
	using Microsoft.Win32;
	using NLog;
    using AirportSimulation.App.Infrastructure;

    public partial class SimulationView : UserControl
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private Action<BuildingComponentType> buildingComponentClicked = delegate { };
		private BlinkingCellsPainter cellsPainter;
		private BitmapImage _currentBuildingComponentImage;
		private BuildingComponentType _currentBuildingComponentType = BuildingComponentType.CheckIn;

		public SimulationView()
		{
			InitializeComponent();
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
			InitializeClickableGridCells();
			ConvertToSettingsService.ClearNodesSerializedData();
            ConvertToSettingsService.ClearNodesCreationData();
		}

		private void Export_Click(object sender, RoutedEventArgs e)
		{
			var data = ConvertToSettingsService.Serialize();

			if (!data.Any())
			{
				MessageBox.Show(
					"There is nothing to be exported! You should draw something on the simulation grid first!");
				return;
			}

			ImportExportSimulation(GetDialogInformation<SaveFileDialog>(), data);
		}
		
		private void Import_Click(object sender, RoutedEventArgs e)
		{
			ImportExportSimulation(GetDialogInformation<OpenFileDialog>(false), null, false);
		}

		private void ImportExportSimulation(
			FileDialog fileDialog,
			IReadOnlyCollection<NodeCreationData> data = null,
			bool export = true)
		{
			fileDialog.ShowDialog();

			if (string.IsNullOrEmpty(fileDialog.FileName))
				return;

			Stream stream = null;

			if (fileDialog is SaveFileDialog saveDialog)
				stream = saveDialog.OpenFile();
			else if (fileDialog is OpenFileDialog openDialog)
				stream = openDialog.OpenFile();

			try
			{
				using (var fs = stream ?? new FileStream("../../simulationImportExportFile", FileMode.OpenOrCreate, FileAccess.ReadWrite))
				{
					if (export)
                    {
                        new BinaryFormatter().Serialize(fs, data ?? new List<NodeCreationData>());
                        ConvertToSettingsService.ClearNodesSerializedData();
                    }
					else
					{
						var bf = new BinaryFormatter();
						var importedData = (List<NodeCreationData>)bf.Deserialize(fs);
						VisualizeSimulationGridFromImportedData(importedData);
						// TODO : VisualizeSimulationGridFromImportedData(importedData)
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		private void VisualizeSimulationGridFromImportedData(List<NodeCreationData> importedData)
		{
			SimulationGrid.Children.Clear();

			for (var index = importedData.Count - 1; index >= 0 ; index--)
			{
				var component = importedData[index];
                foreach((int, int) cell in component.Cell)
                {
                    var (row, col) = cell;

                    var enabledRectangle = new MutantRectangle(cell, new ImageBrush
                    {
                        ImageSource = BuildingComponentsHelper.GetBuildingComponentImage(component.Type),
                        Stretch = Stretch.Fill
                    });
                    //var enabledRectangle = new MutantRectangle(cell);

                    //if(component.Type == BuildingComponentType.Conveyor || component.Type == BuildingComponentType.ManyToOneConveyor)
                    //{
                    //    enabledRectangle.Content = new MultipleCellComponentFactory().CreateComponent(component.Type, cell);
                    //}
                    //else
                    //{
                    //    enabledRectangle.Content = new SingleCellComponentFactory().CreateComponent(component.Type, cell);
                    //}

                    Grid.SetRow(enabledRectangle, row);
                    Grid.SetColumn(enabledRectangle, col);

                    SimulationGrid.Children.Add(enabledRectangle);
                }
			}

            ConvertToSettingsService.SerializedToCreation();
            EnableNextButton();
        }

		private T GetDialogInformation<T>(bool export = true) where T : FileDialog
		{
			FileDialog fileDialog;

			if (export)
				fileDialog = new SaveFileDialog();
			else
				fileDialog = new OpenFileDialog();

			fileDialog.Title = $"{(export ? "Export" : "Import")} Simulation";
			fileDialog.AddExtension = true;
			fileDialog.DefaultExt = "txt";
			fileDialog.Filter = "Text Files (*.txt)|*.txt";

			return (T)fileDialog;
		}
	}
}
