namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using System.Windows;
    using AirportSimulation.App.Infrastructure;
    using System.Windows.Shapes;
    using System.Windows.Media;
    using System.Windows.Input;

    internal class BlinkingCell : GridCell, IClickable 
    {
        public readonly IParent ParentComponent;

        public BlinkingCell(IParent parent, (int, int) cell) : base(cell)
        {
            ParentComponent = parent;
            Fill = RectangleFactory.CreateBlinkingRectangle().Fill;
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type) 
        {
            GenericBuildingComponent content;

            if (IsConveyor(type))  //TODO: Add factory as a property?
            {
                content = new MultipleCellComponentFactory().CreateComponent(type, sender);
                
            }
            else
            {
                content = new SingleCellComponentFactory().CreateComponent(type, sender);
            }

            ParentComponent.ChildClicked(content);

            if(content is IParent parent)
            {
                parent.PopulatePossibleNeighbours(sender);
            }

            sender.ChangeContent(content);
        }

        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            ParentComponent.ShowBlinkingChildren(type);
        }

        private bool IsConveyor(BuildingComponentType type)
        {
            if (type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor)
            {
                return true;
            }
            return false;
        }

        public bool SuccessorAllowed(BuildingComponentType type)
        {
            return ((GenericBuildingComponent)ParentComponent).AllowedNonConveyorSuccessors.Contains(type);
        }
    }
}
