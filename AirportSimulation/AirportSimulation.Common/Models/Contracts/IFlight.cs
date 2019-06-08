namespace AirportSimulation.Common.Models
{
    using System;

    public interface IFlight
    {
        int BaggageCount { get; set; }
        TimeSpan TimeToFlightSinceSimulationStart { get; set; }
        TimeSpan TimeToFlightPreparationSinceSimulationStart { get; }
        string FlightNumber { get; set; }
        string Gate { get; set; }
        string PickUpArea { get; set; }
        int DispatchedBaggageCount { get; set; }
        FlightState FlightState { get; set; }
    }
}