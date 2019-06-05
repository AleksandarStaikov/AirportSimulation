using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
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
            if (successor.GetType().BaseType == typeof(MultipleCellComponent))
            {
                var temp = successor as MultipleCellComponent;
                temp.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                if (temp is ManyToOneCell)
                {
                    ((ManyToOneCell)temp).PredecessorType = this.Type;
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
