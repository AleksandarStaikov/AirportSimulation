using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal abstract class MultipleCellComponent : GenericBuildingComponent, IParent
    {
        protected ISucceedable _succeedable;
        public int Length { get; protected set; }

        public MultipleCellComponent(BuildingComponentType type, string nodeId, (int, int) cell) : base(type, nodeId, cell)
        {
            _succeedable = new Succeedable(this);
        }

        public abstract void ShowBlinkingChildren(BuildingComponentType type); //TODO: Refactor succeedable components. Issues - code repetition

        public virtual void ChildClicked(GenericBuildingComponent successor)
        {
            if(successor is MultipleCellComponent)
            {
                ((MultipleCellComponent)successor).ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                NextNodes.Add(successor);
                Length++;
            }

            ShowBlinkingChildren(successor.Type);
        }

        public void ChangeAllowedSuccessors(List<BuildingComponentType> predecessorAllowed)
        {
            AllowedNonConveyorSuccessors = predecessorAllowed.ToList();
        }

        public void PopulatePossibleNeighbours(MutantRectangle container)
        {
            _succeedable.PopulateAdjacentRectangles(container);
        }
    }
}
