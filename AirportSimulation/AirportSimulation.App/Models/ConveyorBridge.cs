namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.Common;

    internal class ConveyorBridge : SingleCellBuildingComponent, IParent
    {
        public readonly List<MultipleCellComponent> BridgedConveyors;

        public ConveyorBridge((int, int) cell) : base(BuildingComponentType.Bridge, cell)
        {
            BridgedConveyors = new List<MultipleCellComponent>(2);
            successorEnabler = new Succeedable(this);
        }

        public void ChildClicked(GenericBuildingComponent component)
        {
            var conveyor = BridgedConveyors.FirstOrDefault(c => c.NodeId == component.NodeId);

            if (component.AllowedNonConveyorSuccessors != null)
            {
                if(component is IParent parent)
                {
                    var bridgedConveyor = new MultipleCellComponentFactory().CreateComponent(component.Type, this.Cell);
                    parent.ChildClicked(bridgedConveyor);
                }
            }
            else
            {
                var predecessor = PossibleNeighbours.FirstOrDefault(p => p.Content is GenericBuildingComponent c && c.NodeId != BridgedConveyors[0].NodeId).Content;

                //var componentContainer = PossibleNeighbours.FirstOrDefault(c => c.Cell.Equals(component.Cell));
                var bridgedConveyor = new MultipleCellComponentFactory().CreateComponent(component.Type, this.Cell);
                var bridgedConveyorBlinkingCell = new BlinkingCell(bridgedConveyor as IParent, component.Cell);
                
                if(predecessor is IParent parent)
                {
                    parent.ChildClicked(bridgedConveyor);
                    ((IParent)bridgedConveyor).ChildClicked(component);
                }
            }            
        }

        public void PopulatePossibleNeighbours(MutantRectangle container)
        {
            successorEnabler.PopulateAdjacentRectangles(container);
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            if ((type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor)
                && BridgedConveyors.Capacity != BridgedConveyors.Count)
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
