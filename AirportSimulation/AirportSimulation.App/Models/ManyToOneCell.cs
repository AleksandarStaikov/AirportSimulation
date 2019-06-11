namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using AirportSimulation.Common;
    
    internal class ManyToOneCell : MultipleCellComponent
    {
        public BuildingComponentType PredecessorType;

        public ManyToOneCell((int, int) cell) : base(BuildingComponentType.ManyToOneConveyor,cell)
        {
        }

        public override void ShowBlinkingChildren(BuildingComponentType type) //TODO: review repeated code
        {
            if (((PredecessorType == type && NextNodes.Count != 0) != 
                (NextNodes.Count == 0 && (AllowedNonConveyorSuccessors.Contains(type) || type == this.Type)))) //TODO: Simplify expression //TODO: Many SingleCell to one MultiCell
            {
                successorEnabler.ShowBlinkingCells();
            }
            else
            {
                successorEnabler.HideBlinkingCells();
            }
        }

        public override void ChildClicked(GenericBuildingComponent successor)
        {
            if (successor is SingleCellBuildingComponent && NextNodes.Count != 0)
            {
                ((IParent)successor).ChildClicked(this);
            }

            base.ChildClicked(successor);
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
                {
                    PredecessorType
                };
        }
    }
}
