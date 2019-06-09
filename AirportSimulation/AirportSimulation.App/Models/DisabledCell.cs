namespace AirportSimulation.App.Models
{
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.Common;
    
    internal class DisabledCell : GridCell, IClickable
    {
        public readonly GenericBuildingComponent ParentComponent;
        public DisabledCell((int, int) cell) : base(cell)
        {
        }

        public DisabledCell(IParent parentComponent, (int, int) cell) : base(cell)
        {
            ParentComponent = parentComponent as GenericBuildingComponent;
            Fill = RectangleFactory.CreateDisabledRectangle().Fill;
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {

        }

        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if (ParentComponent != null && ParentComponent is IParent)
            {
                ((IParent)ParentComponent).ShowBlinkingChildren(type);
            }

            if (type == BuildingComponentType.CheckIn && ParentComponent == null)
            {
                var content = new EnabledCell(Cell);
                content.Fill = RectangleFactory.CreateEnabledRectangle().Fill;
                sender.ChangeContent(content);
            }
        }
    }
}
