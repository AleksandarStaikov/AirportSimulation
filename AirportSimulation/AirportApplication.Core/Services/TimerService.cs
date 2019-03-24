namespace AirportSimulation.Core.Services
{
    using Abstractions.Contracts;
    using System;
    using System.Diagnostics;

    public class TimerService : ITimerService
    {

        private int _multiplier;
        private Stopwatch _watch;

        public TimerService()
        {
            this._watch = new Stopwatch();
        }

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
            var elapsedTicks = _watch.ElapsedTicks;
            elapsedTicks *= _multiplier;
            return new TimeSpan(elapsedTicks);
        }
    }
}
