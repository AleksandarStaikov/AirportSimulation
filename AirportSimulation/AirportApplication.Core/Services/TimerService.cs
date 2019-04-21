namespace AirportSimulation.Core.Services
{
    using Abstractions.Contracts;
    using Common.Models;
    using System;
    using System.Diagnostics;

    public class TimerService : ITimerService
    {
        private SimulationSettings _simulationSettings;

        private Stopwatch _watch;

        public TimerService()
        {
            _watch = new Stopwatch();
        }

        public float SimulationMultiplier => _simulationSettings.Multiplier;

        public void RunNewTimer()
        {
            _watch.Restart();
        }

        public void StopTimer()
        {
            _watch.Stop();
        }

        public void ResumeTimer()
        {
            _watch.Start();
        }

        public TimeSpan GetTimeSinceSimulationStart()
        {
            var elapsedTicks = _watch.Elapsed.Ticks;
            elapsedTicks = (long)Math.Round((float)elapsedTicks * _simulationSettings.Multiplier);
            return new TimeSpan(elapsedTicks);
        }

        public long GetTicksSinceSimulationStart()
        {
            var elapsedTicks = _watch.Elapsed.Ticks;
            return (long)(elapsedTicks * _simulationSettings.Multiplier);
        }

        public TimeSpan ConvertMillisecondsToTimeSpan(int milliseconds)
        {
            return new TimeSpan(ticks: milliseconds * 10000);
        }

        public int ConvertTimeSpanToMilliseconds(TimeSpan span)
        {
            return (int)span.Ticks / 10000;
        }

        public void SetSettings(SimulationSettings settings)
        {
            _simulationSettings = settings;

        }
    }
}
