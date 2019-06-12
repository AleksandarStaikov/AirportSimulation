namespace AirportSimulation.Abstractions.Contracts
{
    using Common.Models;
    using Common.Models.Contracts;
    using System;

    public interface ITimerService : IPauseResume
    {
        Action<Flight> FlightLandedEvent { get; set; }
        Action<Flight> PrepareFlightEvent { get; set; }

        float SimulationMultiplier { get; }
        void RunNewTimer();
        TimeSpan GetTimeSinceSimulationStart();
        long GetTicksSinceSimulationStart();
        TimeSpan ConvertMillisecondsToTimeSpan(int milliseconds);
        int ConvertTimeSpanToMilliseconds(TimeSpan span);
        void SetSettings(ISimulationSettings settings);
    }
}