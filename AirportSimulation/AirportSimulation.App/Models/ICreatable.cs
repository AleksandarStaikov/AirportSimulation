namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common.Models;

    internal interface ICreatable
    {
        NodeCreationData GetCreationData();
    }
}
