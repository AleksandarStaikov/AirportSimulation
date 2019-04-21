namespace AirportSimulation.Common.Models.Contracts
{
    using System.Collections.Generic;

    public interface IMultiplierSettings
    {
        int Multiplier { get; set; }
    }

    public interface IFlightManagement : IMultiplierSettings
    {
        List<Flight> Flights { get; set; }
    }
}
