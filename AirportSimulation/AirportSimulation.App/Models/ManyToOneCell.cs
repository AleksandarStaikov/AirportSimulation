using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    class ManyToOneCell : MultipleCellComponent
    {
        public BuildingComponentType PredecessorType;
        public ManyToOneCell((int, int) cell) : base(BuildingComponentType.ManyToOneConveyor,cell)
        {
        }

        public override void ShowBlinkingChildren(BuildingComponentType type) //TODO: review repeated code
        {
            if (((AllowedNonConveyorSuccessors.Contains(type) && NextNodes.Count != 0) != 
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
