namespace AirportSimulation.App.Helpers
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Imaging;
    using Common.Models;
    using AirportSimulation.App.Models;

    internal static class BuildingComponentsHelper
    {
        private static readonly Dictionary<BuildingComponentType, List<BuildingComponentType>> _allowedSingleCellComponentsPerComponent
            = new Dictionary<BuildingComponentType, List<BuildingComponentType>>()
            {
                {
                    BuildingComponentType.CheckIn,
                    new List<BuildingComponentType>(){ BuildingComponentType.PSC }
                },
                {
                    BuildingComponentType.PSC,
                    new List<BuildingComponentType>(){ BuildingComponentType.MPA, BuildingComponentType.ASC }
                },
                {
                    BuildingComponentType.ASC,
                    new List<BuildingComponentType>(){ BuildingComponentType.MPA }
                },
                {
                    BuildingComponentType.MPA,
                    new List<BuildingComponentType>(){ BuildingComponentType.AA }
                },
            };

        private static readonly Dictionary<BuildingComponentType, string> _buildingComponentsImages =
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

        private static readonly Dictionary<int, BuildingComponentType> _buildingComponentsSteps =
            new Dictionary<int, BuildingComponentType>
            {
                {1, BuildingComponentType.CheckIn},
                {2, BuildingComponentType.ManyToOneConveyor },
                {3, BuildingComponentType.PSC},
                {4, BuildingComponentType.Conveyor},
                {5, BuildingComponentType.MPA},
                {6, BuildingComponentType.Conveyor },
                {7, BuildingComponentType.AA },
                {8, BuildingComponentType.PA }
            };

        public static (BuildingComponentType, BitmapImage) EnableNextComponentButtonAndGetTypeAndImage(SimulationGridOptions simulationGridOptions, int step, bool isFirstTime = false)
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

        public static void DisableComponentsButtons(SimulationGridOptions simulationGridOptions)
        {
            var properties = simulationGridOptions.GetType().GetProperties().ToList();
            var canBuildProperties = properties.Where(x => x.Name.Contains("Build")).ToList();

            foreach (var prop in canBuildProperties)
            {
                prop.SetValue(simulationGridOptions, false, null);
            }
        }

        public static List<BuildingComponentType> GetAllowedComponents(BuildingComponentType predecessorType)
        {
            return _allowedSingleCellComponentsPerComponent[predecessorType];
        }

        public static BitmapImage GetBuildingComponentImage(BuildingComponentType type) =>
            GetComponentImage(_buildingComponentsImages[type]);

        private static (BuildingComponentType, BitmapImage) GetNextBuildingComponentTypeAndImage(int step)
            => (_buildingComponentsSteps[step], GetBuildingComponentImage(_buildingComponentsSteps[step]));

        private static BitmapImage GetComponentImage(string fileLocation)
            => new BitmapImage(new Uri($"../../{fileLocation}", UriKind.Relative));

        public static readonly List<GenericBuildingComponent> BuiltList = new List<GenericBuildingComponent>();
    }
}
