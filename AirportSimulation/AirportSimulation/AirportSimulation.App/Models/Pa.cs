namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;
    
    internal class Pa : SingleCellBuildingComponent
    {
        public Pa((int, int) cell) : base(BuildingComponentType.PA, cell)
        {
        }
    }
}
