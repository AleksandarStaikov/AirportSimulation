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

        public DisabledCell(IParent parentComponent, (int, int) cell) : base(cell)
        {
            _parentComponent = parentComponent as GenericBuildingComponent;
            Fill = RectangleFactory.CreateDisabledRectangle().Fill;
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

            if (type == BuildingComponentType.CheckIn && _parentComponent == null)
            {
                var content = new EnabledCell(Cell);
                content.Fill = RectangleFactory.CreateEnabledRectangle().Fill;
                sender.ChangeContent(content);
            }
        }
    }
}
