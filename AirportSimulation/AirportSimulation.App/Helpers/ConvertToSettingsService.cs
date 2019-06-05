using AirportSimulation.App.Models;
using AirportSimulation.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulation.App.Helpers
{
    internal static class ConvertToSettingsService
    {
        public readonly static List<GenericBuildingComponent> StartNodes = new List<GenericBuildingComponent>();
        public readonly static List<string> Listed = new List<string>();
        public readonly static List<NodeCreationData> NodesCreationData = new List<NodeCreationData>();

        public static IEnumerable<NodeCreationData> Convert()
        {
            foreach(GenericBuildingComponent startingNode in StartNodes)
            {
                if(startingNode is ICreatable creatableNode)
                {
                    creatableNode.GetCreationData();
                }
            }

            return NodesCreationData.AsEnumerable();
        }

        private static void DFS(GenericBuildingComponent node)
        {
            if(node is ICreatable creatableNode)
            {
                var data = ((ICreatable)node).GetCreationData();
                NodesCreationData.Add(data);

                Listed.Add(node.NodeId);
            }
            
            foreach(GenericBuildingComponent nextNode in node.NextNodes)
            {
                if (!Listed.Contains(nextNode.NodeId))
                {
                    DFS(nextNode);
                }
            }
        }
    }
}
