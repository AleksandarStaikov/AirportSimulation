namespace AirportSimulation.App.Helpers
{
	using Common;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Media.Imaging;
	using Common.Models;

	internal class BuildingComponentsHelper
	{
		private readonly Dictionary<BuildingComponentType, string> _buildingComponentsImages =
			new Dictionary<BuildingComponentType, string>
			{
				{BuildingComponentType.CheckIn, "Resources/check-in.png"},
				{BuildingComponentType.Conveyor, "Resources/conveyor.png"},
				{BuildingComponentType.PA, "Resources/PickUpBaggage.png"},
				{BuildingComponentType.PSC, "Resources/PSCbaggage.png"},
				{BuildingComponentType.ASC, "Resources/AdvancedCheckBaggage.png"},
				{BuildingComponentType.AA, "Resources/airplane-shape.png"},
				{BuildingComponentType.MPA, "Resources/MPApng.png"},
				{BuildingComponentType.ManyToOneConveyor, "Resources/manytomanyConv.png"}
			};

		private readonly Dictionary<int, BuildingComponentType> _buildingComponentsSteps =
			new Dictionary<int, BuildingComponentType>
			{
				{1, BuildingComponentType.CheckIn},
				{2, BuildingComponentType.ManyToOneConveyor },
				{3, BuildingComponentType.PSC},
				{4, BuildingComponentType.Conveyor},
				{5, BuildingComponentType.MPA},
				{6, BuildingComponentType.Conveyor },
				{7, BuildingComponentType.AA }
			};

		public (BuildingComponentType, BitmapImage) EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions simulationGridOptions, int step, bool isFirstTime = false)
		{
			if (!isFirstTime)
			{
				DisableComponentsButtons(simulationGridOptions);
			}

			switch (step)
			{
				case 1:
					simulationGridOptions.CanBuildCheckIn = true;
					break;
				case 2:
					simulationGridOptions.CanBuildManyToOneConveyor = true;
					break;
				case 3:
					simulationGridOptions.CanBuildPsc = true;
					break;
				case 4:
					simulationGridOptions.CanBuildConveyor = true;
					break;
				case 5:
					simulationGridOptions.CanBuildMpa = true;
					break;
				case 6:
					simulationGridOptions.CanBuildConveyor = true;
					break;
				case 7:
					simulationGridOptions.CanBuildAa = true;
					break;
			}

			return GetNextBuildingComponentTypeAndImage(step);
		}

		public void DisableComponentsButtons(SimulationGridOptions simulationGridOptions)
		{
			var properties = simulationGridOptions.GetType().GetProperties().ToList();
			var canBuildProperties = properties.Where(x => x.Name.Contains("Build")).ToList();

			foreach (var prop in canBuildProperties)
			{
				prop.SetValue(simulationGridOptions, false, null);
			}
		}

		public BitmapImage GetBuildingComponentImage(BuildingComponentType type) =>
			GetComponentImage(_buildingComponentsImages[type]);

		private (BuildingComponentType, BitmapImage) GetNextBuildingComponentTypeAndImage(int step)
			=> (_buildingComponentsSteps[step], GetBuildingComponentImage(_buildingComponentsSteps[step]));

		private static BitmapImage GetComponentImage(string fileLocation)
			=> new BitmapImage(new Uri($"../../{fileLocation}", UriKind.Relative));
	}
}
