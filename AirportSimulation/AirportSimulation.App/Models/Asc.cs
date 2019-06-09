namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using AirportSimulation.Common;

    internal class Asc : SingleCellBuildingComponent, IParent
    {
        public Asc((int, int) cell) : base(BuildingComponentType.ASC, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.MPA
            };

            successorEnabler = new Succeedable(this);
            NextNodes = new List<GenericBuildingComponent>(1);
        }

        public void ChildClicked(GenericBuildingComponent successor)
        {
            if (successor.GetType().BaseType == typeof(MultipleCellComponent))
            {
                if(successor.AllowedNonConveyorSuccessors == null)
                {
                    var temp = successor as MultipleCellComponent;
                    temp.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);

                    if (temp is ManyToOneCell)
                    {
                        ((ManyToOneCell)temp).PredecessorType = this.Type;
                    }

                    NextNodes.Add(successor);
                }
                else
                {
                    if(successor is IParent parentComponent)
                    {
                        parentComponent.ChildClicked(this);
                    }
                }
            }
            

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
