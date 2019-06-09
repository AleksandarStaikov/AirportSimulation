﻿namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using AirportSimulation.App.Helpers;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    
    internal abstract class MultipleCellComponent : GenericBuildingComponent, IParent
    {
        public int Index { get; protected set; } = 0;

        public MultipleCellComponent(BuildingComponentType type, (int, int) cell) : base(type, cell)
        {
            successorEnabler = new Succeedable(this);
        }

        public abstract void ShowBlinkingChildren(BuildingComponentType type); //TODO: Refactor succeedable components. Issues - code repetition

        public virtual void ChildClicked(GenericBuildingComponent successor)
        {
            if(successor is MultipleCellComponent component)
            {
                component.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                component.Index++;
            }

            NextNodes.Add(successor);
            ShowBlinkingChildren(successor.Type);
        }

        public void ChangeAllowedSuccessors(List<BuildingComponentType> predecessorAllowed)
        {
            AllowedNonConveyorSuccessors = predecessorAllowed.ToList();
        }

        public void PopulatePossibleNeighbours(MutantRectangle container)
        {
            successorEnabler.PopulateAdjacentRectangles(container);
        }

        public override NodeCreationData GetCreationData()
        {
            NodeCreationData nodeData = null;
            if (!ConvertToSettingsService.Listed.Contains(this.NodeId))
            {
                ConvertToSettingsService.Listed.Add(this.NodeId);

                nodeData = new NodeCreationData();
                Dictionary<NodeCreationData, int?> nextNodesData = new Dictionary<NodeCreationData, int?>();

                var lastSegment = GetLastSegment();

                nodeData.Id = lastSegment.NodeId;

                foreach (ICreatable nextNode in lastSegment.NextNodes)
                {
                    nextNodesData.Add(nextNode.GetCreationData(), null);
                }
                nodeData.NextNodes = nextNodesData;

                nodeData.Type = lastSegment.Type;
                nodeData.Length = lastSegment.Index + 1;
            }
            else
            {
                nodeData = ConvertToSettingsService.NodesCreationData.FirstOrDefault(data => data.Id == this.NodeId);
            }

            ConvertToSettingsService.NodesCreationData.Add(nodeData);
            return nodeData;
        }

        private MultipleCellComponent GetLastSegment()
        {
            var tempCell = this;

            if (tempCell.NextNodes[0] == null)
            {
                return tempCell;
            }

            while(!(tempCell.NextNodes[0] is SingleCellBuildingComponent)) //TODO: Fix index out of bounds
            {
                tempCell = tempCell.NextNodes[0] as MultipleCellComponent;
            }

            return tempCell;
        }
    }
}
