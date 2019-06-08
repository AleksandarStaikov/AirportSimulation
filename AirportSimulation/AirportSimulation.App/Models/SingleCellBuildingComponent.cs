namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;
       
    internal abstract class SingleCellBuildingComponent : GenericBuildingComponent
    {
        public GenericBuildingComponent Predecessor { get; set; }

        public SingleCellBuildingComponent(BuildingComponentType type, (int, int) cell) 
            : base(type, cell)
        {
            
        }

        //public NodeCreationData GetCreationData()
        //{
        //    NodeCreationData nodeData = null;
        //    if (!ConvertToSettingsService.Listed.Contains(this.NodeId))
        //    {
        //        ConvertToSettingsService.Listed.Add(this.NodeId);
        //        nodeData = new NodeCreationData
        //        {
        //            Id = this.NodeId,
        //            Type = this.Type
        //        };
        //        Dictionary<NodeCreationData, int?> nextNodesData = new Dictionary<NodeCreationData, int?>();

        //        int? index = null;

        //        foreach (ICreatable nextNode in this.NextNodes)
        //        {
        //            if (nextNode is ManyToOneCell manyToOne)
        //            {
        //                index = manyToOne.Index;
        //            }
        //            nextNodesData.Add(nextNode.GetCreationData(), index ?? null);
        //        }

        //        nodeData.NextNodes = nextNodesData;
        //    }
        //    else
        //    {
        //        nodeData = ConvertToSettingsService.NodesCreationData.FirstOrDefault(data => data.Id == this.NodeId);
        //    }

        //    ConvertToSettingsService.NodesCreationData.Add(nodeData);
        //    return nodeData;
        //}
    }
}
