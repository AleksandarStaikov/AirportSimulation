namespace AirportSimulation.Abstractions.Contracts
{
    using System;
    using Common.Models;

    public interface ITimerService
    {
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