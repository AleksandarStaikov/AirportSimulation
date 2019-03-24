namespace AirportSimulation.Abstractions.Contracts
{
    using System;

    public interface ITimerService
    {
        void RunNewTimer(int multiplier);
        void StopTimer();
        void ResumeTimer();
        TimeSpan GetTimeSinceSimulationStart();
    }
}