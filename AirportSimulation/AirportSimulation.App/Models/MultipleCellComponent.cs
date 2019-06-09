using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AirportSimulation.App.Helpers;
using AirportSimulation.App.Infrastructure;
using AirportSimulation.Common;
using AirportSimulation.Common.Models;

namespace AirportSimulation.App.Models
{
    internal abstract class MultipleCellComponent : GenericBuildingComponent, IParent, IClickable
    {
        public int Index { get; protected set; } = 0;

        public MultipleCellComponent(BuildingComponentType type, (int, int) cell) : base(type, cell)
        {
            NodeId = Guid.NewGuid().ToString();
            successorEnabler = new Succeedable(this);
        }

        public abstract void ShowBlinkingChildren(BuildingComponentType type); //TODO: Refactor succeedable components. Issues - code repetition

        public virtual void ChildClicked(GenericBuildingComponent successor)
        {
            if (successor is MultipleCellComponent component)
            {
                component.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                component.Index = Index + 1;
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
            if (!ConvertToSettingsService.ListedForCreation.Contains(this.NodeId))
            {
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

                ConvertToSettingsService.ListedForCreation.Add(this.NodeId);
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

            while (!(tempCell.NextNodes[0] is SingleCellBuildingComponent))
            {
                tempCell = tempCell.NextNodes[0] as MultipleCellComponent;
            }

            return tempCell;
        }


        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            var grid = sender.GetGrid();

            if (type == BuildingComponentType.Bridge)
            {
                sender.Fill = RectangleFactory.CreateBlinkingRectangle().Fill;
            }
            else
            {
                sender.Fill = this.Fill;
            }
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if (type == BuildingComponentType.Bridge)
            {
                var bridge = new SingleCellComponentFactory().CreateComponent(type, sender.Cell) as ConveyorBridge;

                bridge.BridgedConveyors.Add(this);
                bridge.PopulatePossibleNeighbours(sender);
                sender.ChangeContent(bridge);
            }
        }
    }
}
