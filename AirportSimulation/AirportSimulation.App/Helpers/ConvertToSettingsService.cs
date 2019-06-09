namespace AirportSimulation.App.Helpers
{
    using AirportSimulation.App.Models;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    internal static class ConvertToSettingsService
    {
        public readonly static List<GenericBuildingComponent> StartNodes = new List<GenericBuildingComponent>();
        public readonly static List<string> Listed = new List<string>();
        public readonly static List<NodeCreationData> NodesCreationData = new List<NodeCreationData>();

        public static IEnumerable<NodeCreationData> Convert()
        {
            foreach (GenericBuildingComponent startingNode in StartNodes)
            {
                if (startingNode is ICreatable creatableNode)
                {
                    creatableNode.GetCreationData();
                }
            }

            return NodesCreationData.AsEnumerable();
        }

        public static ObservableCollection<string> GetAvailablePickUpAreas() => GetByType(BuildingComponentType.PA, "P");

        public static ObservableCollection<string> GetAvailableGates() => GetByType(BuildingComponentType.AA, "A");

        private static ObservableCollection<string> GetByType(BuildingComponentType type, string prefix)
        {
            var oc = new ObservableCollection<string>();
            var count = NodesCreationData.Count(n => n.Type == type);

            for (int i = 1; i <= count; i++)
                oc.Add($"{prefix}{i}");

            return oc;
        }
    }
}
