using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.App.Infrastructure;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    class DisabledCell : GridCell, IClickable
    {
        private GenericBuildingComponent _parentComponent;
        public DisabledCell((int, int) cell) : base(cell)
        {
            UIElement = RectangleFactory.CreateDisabledRectangle();
        }

        public DisabledCell(GenericBuildingComponent parentComponent, (int, int) cell) : base(cell)
        {
            _parentComponent = parentComponent;
            UIElement = RectangleFactory.CreateDisabledRectangle();
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {

        }

        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if (_parentComponent != null && _parentComponent is IParent)
            {
                ((IParent)_parentComponent).ShowBlinkingChildren(type);
            }

            if (type == BuildingComponentType.CheckIn)
            {
                var content = new EnabledCell(Cell);
                content.Fill = RectangleFactory.CreateEnabledRectangle().Fill;
                sender.ChangeContent(content);
            }
        }
    }
}
