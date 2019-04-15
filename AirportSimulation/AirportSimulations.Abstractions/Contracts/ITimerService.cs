namespace AirportSimulation.Abstractions.Contracts
{
    using System;

    public interface ITimerService
    {
        float SimulationMultiplier { get; }
        void RunNewTimer(int multiplier);
        void StopTimer();
        void ResumeTimer();
        TimeSpan GetTimeSinceSimulationStart();
        long GetTicksSinceSimulationStart();
        TimeSpan ConvertMillisecondsToTimeSpan(int milliseconds);
    }
}