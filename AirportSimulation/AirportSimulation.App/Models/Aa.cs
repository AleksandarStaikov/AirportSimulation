namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;

    internal class Aa : SingleCellBuildingComponent
    {
        public Aa((int, int) cell) : base(BuildingComponentType.AA, cell)
        {
        }
    }
}
