namespace AirportSimulation.Abstractions.Contracts
{
    using Common.Models;
    using System;

    public interface ITimerService
    {
        Action<Flight> FlightLandedEvent { get; set; }
        Action<Flight> PrepareFlightEvent { get; set; }

        float SimulationMultiplier { get; }
        void RunNewTimer();
        void StopTimer();
        void ResumeTimer();
        TimeSpan GetTimeSinceSimulationStart();
        long GetTicksSinceSimulationStart();
        TimeSpan ConvertMillisecondsToTimeSpan(int milliseconds);
        int ConvertTimeSpanToMilliseconds(TimeSpan span);
        void SetSettings(SimulationSettings settings);
    }
}