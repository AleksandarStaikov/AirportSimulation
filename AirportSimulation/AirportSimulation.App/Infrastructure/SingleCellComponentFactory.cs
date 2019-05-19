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
        private BuildingComponentsHelper _buildingComponentsHelper;

        public SingleCellComponentFactory()
        {
            _buildingComponentsHelper = new BuildingComponentsHelper();
        }

        public void CreateMultipleCellComponent(BuildingComponentType type)
        {
            throw new NotImplementedException();
        }

        public SingleCellBuildingComponent CreateSingleCellComponent(BuildingComponentType type, string nodeId, (int, int) cell)
        {
            return new SingleCellBuildingComponent(type, nodeId, cell)
            {
                UIElement = RectangleFactory.CreateBuildingComponentRectangle(_buildingComponentsHelper.GetBuildingComponentImage(type))
            };
        }
    }
}
