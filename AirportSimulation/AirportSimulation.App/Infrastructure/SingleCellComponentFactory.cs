namespace AirportSimulation.App.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using AirportSimulation.App.Models;
    using AirportSimulation.App.Helpers;

    internal class SingleCellComponentFactory : IBuildingComponentFactory
    {
        public SingleCellComponentFactory()
        {
            
        }

        public GenericBuildingComponent CreateComponent(BuildingComponentType type, (int, int) cell)
        {
            if(type == BuildingComponentType.CheckIn)
            {
                return CreateCheckIn(cell);
            }

            return null;
        }

        private CheckIn CreateCheckIn((int, int) cell)
        {
            return new CheckIn("asdas", cell)
            {
                UIElement = RectangleFactory.CreateBuildingComponentRectangle(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn))
            };
        } 
    }
}
