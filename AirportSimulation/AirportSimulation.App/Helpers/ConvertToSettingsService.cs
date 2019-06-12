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
        public static readonly List<GenericBuildingComponent> StartNodes = new List<GenericBuildingComponent>();
        public static readonly List<string> ListedForCreation = new List<string>();
        public static readonly List<string> ListedForSerialization = new List<string>();
        public static readonly List<NodeCreationData> NodesCreationData = new List<NodeCreationData>();
        public static readonly List<NodeCreationData> NodesSerializedData = new List<NodeCreationData>();

        public static IEnumerable<NodeCreationData> ConvertToCreationData()
        {
            foreach (GenericBuildingComponent startingNode in StartNodes)
            {
                if (startingNode is ICreatable creatableNode)
                {
                    creatableNode.GetCreationData();
                    creatableNode.GetSerializedData();
                }
            }

            return NodesCreationData.AsEnumerable();
        }

        public static List<NodeCreationData> Serialize()
        {
            foreach (GenericBuildingComponent startingNode in StartNodes)
            {
                if (startingNode is ICreatable creatableNode)
                {
                    creatableNode.GetSerializedData();
                }
            }

            return NodesSerializedData;
        }

        public static List<NodeCreationData> SerializedToCreation(List<NodeCreationData> importedData)
        {
            var serializedData = importedData;
            serializedData.Reverse();
            int lenght = 0;

            foreach (NodeCreationData component in serializedData)
            {
                if(!ListedForCreation.Contains(component.Id))
                {
                    lenght++;
                    var nextNode = component.NextNodes.Keys.FirstOrDefault();
                    if (nextNode == null || nextNode.Type != component.Type)
                    {
                        NodeCreationData nodeData = new NodeCreationData();

                        nodeData.Id = component.Id;
                        nodeData.Length = lenght;
                        if (component.ConveyorIndex != null)
                        {
                            nodeData.ConveyorIndex = component.ConveyorIndex;
                        }
                        nodeData.NextNodes = component.NextNodes;
                        nodeData.Type = component.Type;

                        lenght = 0;
                        NodesCreationData.Add(nodeData);
                        //ListedForCreation.Add(component.Id);
                    }
                }
            }

            return NodesCreationData;
        }

		public static void ClearNodesSerializedData()
		{
			ListedForSerialization.Clear();
			NodesSerializedData.Clear();
		}

        public static void ClearNodesCreationData()
        {
            ListedForCreation.Clear();
            NodesCreationData.Clear();
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
