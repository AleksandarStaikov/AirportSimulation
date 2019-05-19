namespace AirportSimulation.App.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using AirportSimulation.App.Models;


    internal interface IBuildingComponentFactory
    {
        SingleCellBuildingComponent CreateSingleCellComponent(BuildingComponentType type, string nodeId, (int, int) cell);
        void CreateMultipleCellComponent(BuildingComponentType type);
    }
}
