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
	using Microsoft.Win32;
	using NLog;

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
						new BinaryFormatter().Serialize(fs, data ?? new List<NodeCreationData>());
					else
					{
						var bf = new BinaryFormatter();
						var importedData = (List<NodeCreationData>)bf.Deserialize(fs);

						// TODO : VisualizeSimulationGridFromImportedData(importedData)
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
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
