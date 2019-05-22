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

    class BlinkingCell : GridCell
    {
        public readonly GenericBuildingComponent ParentComponent;

        public BlinkingCell(GenericBuildingComponent parent, (int, int) cell) : base(cell)
        {
            ParentComponent = parent;
            UIElement = RectangleFactory.CreateBlinkingRectangle();
        }

        public override void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            
        }

        public void HideBlinkingCellBasedOnComponentType(BuildingComponentType selectedType)
        {
            if (TypeAllowed(selectedType))
            {
                UIElement = RectangleFactory.CreateBlinkingRectangle();
            }
            else
            {
                UIElement = RectangleFactory.CreateDisabledRectangle();
            }
        }

        public bool TypeAllowed(BuildingComponentType type)
        {
            return ParentComponent.AllowedSuccessors.Contains(type);
        }
    }
}
