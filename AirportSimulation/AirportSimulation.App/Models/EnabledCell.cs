namespace AirportSimulation.App.Models
{
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.Common;
    
    internal class EnabledCell : GridCell, IClickable
    {
        public EnabledCell((int, int) cell) : base(cell)
        {
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if(type == BuildingComponentType.CheckIn)
            {
                
            }
            var content = new SingleCellComponentFactory().CreateComponent(type, sender);
            sender.ChangeContent(content);
        }

        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if(type != BuildingComponentType.CheckIn && type != BuildingComponentType.PSC)
            {
                var content = new DisabledCell(Cell)
                {
                    Fill = RectangleFactory.CreateDisabledRectangle().Fill
                };
                sender.ChangeContent(content);
            }
        }
    }
}
