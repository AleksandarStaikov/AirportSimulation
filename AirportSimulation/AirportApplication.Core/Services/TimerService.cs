namespace AirportSimulation.Core.Services
{
    using Abstractions.Contracts;
    using System;
    using System.Diagnostics;

    public class TimerService : ITimerService
    {
        private const float DefaultSimulationMultiplier = 1;

        private float _multiplier;
        private Stopwatch _watch;

        public TimerService()
        {
            this._watch = new Stopwatch();
            _multiplier = DefaultSimulationMultiplier;
        }

        public float SimulationMultiplier => _multiplier;

        public void RunNewTimer(int multiplier)
        {
            _multiplier = multiplier;
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
            elapsedTicks = (long)Math.Round((float)elapsedTicks / _multiplier);
            return new TimeSpan(elapsedTicks);
        }

        public long GetTicksSinceSimulationStart()
        {
            var elapsedTicks = _watch.Elapsed.Ticks;
            return (long)(elapsedTicks * _multiplier);
        }

        public TimeSpan ConvertMillisecondsToTimeSpan(int milliseconds)
        {
            return new TimeSpan(ticks: milliseconds * 10000);
        }
    }
}
