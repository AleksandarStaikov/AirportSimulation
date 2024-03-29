﻿namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using AirportSimulation.Common;
    
    internal class Psc : SingleCellBuildingComponent, IParent
    {
        public Psc((int, int) cell) : base(BuildingComponentType.PSC, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.ASC, BuildingComponentType.MPA
            };
            successorEnabler = new Succeedable(this);
            NextNodes = new List<GenericBuildingComponent>(2);
        }

        public void ChildClicked(GenericBuildingComponent successor)
        {
            if (successor is MultipleCellComponent component)
            {
                component.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                if (component is ManyToOneCell)
                {
                    ((ManyToOneCell)component).PredecessorType = this.Type;
                }
            }
            NextNodes.Add(successor);

            ShowBlinkingChildren(successor.Type);
        }

        public void PopulatePossibleNeighbours(MutantRectangle container)
        {
            successorEnabler.PopulateAdjacentRectangles(container);
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            if ((type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor)
                && NextNodes.Capacity != NextNodes.Count)
            {
                successorEnabler.ShowBlinkingCells();
            }
            else
            {
                successorEnabler.HideBlinkingCells();
            }
        }
    }
}
