namespace AirportSimulation.Core.Services
{
    using Abstractions.Contracts;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Windows.Threading;

    public class TimerService : ITimerService
    {
        private SimulationSettings _simulationSettings;
        private Dispatcher _mainThread;

        private Stopwatch _watch;
        private Timer _landingFlightsWatcher;
        private Timer _preparationFlightsWatcher;

        public TimerService()
        {
            _watch = new Stopwatch();
            _mainThread = Dispatcher.CurrentDispatcher;
        }

        public float SimulationMultiplier => _simulationSettings.Multiplier = 8;

        public void RunNewTimer()
        {
            _watch.Restart();
            int m = (int)(1000 / SimulationMultiplier);
            _landingFlightsWatcher = new Timer(CheckForIncomingFlights, _simulationSettings.IncomingFlights, m, m);
            _preparationFlightsWatcher = new Timer(CheckForFlightPreparations, _simulationSettings.OutgoingFlights, m, m);
        }

        public void StopTimer()
        {
            _watch.Stop();
            _landingFlightsWatcher.Dispose();
            _preparationFlightsWatcher.Dispose();
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
            _simulationSettings.OutgoingFlights = _simulationSettings
                .OutgoingFlights.OrderBy(f => f.TimeToFlightSinceSimulationStart)
                .ToList();
            _simulationSettings.IncomingFlights = _simulationSettings
                .IncomingFlights
                .OrderBy(f => f.TimeToFlightSinceSimulationStart)
                .ToList();
        }

        private void CheckForIncomingFlights(object state)
        {
            var incomingFlights = (List<Flight>)state;
            foreach (var flight in incomingFlights.Where(f => f.FlightState == FlightState.Incoming))
            {
                if (flight.TimeToFlightSinceSimulationStart < GetTimeSinceSimulationStart())
                {
                    flight.FlightState = FlightState.Landed;
                    _mainThread.BeginInvoke((Action)(() =>
                    {
                        FlightLandedEvent?.Invoke(flight);
                    }));
                }
            }
        }

        private void CheckForFlightPreparations(object state)
        {
            var outgoingFlights = (List<Flight>)state;
            foreach (var flight in outgoingFlights.Where(f => f.FlightState == FlightState.WaitingForPreparation))
            {
                if (flight.TimeToFlightPreparationSinceSimulationStart < GetTimeSinceSimulationStart())
                {
                    flight.FlightState = FlightState.InPreparation;
                    _mainThread.BeginInvoke((Action)(() =>
                    {
                        PrepareFlightEvent?.Invoke(flight);
                    }));
                }
            }

            foreach (var flight in outgoingFlights.Where(f => f.FlightState == FlightState.InPreparation))
            {
                if (flight.TimeToFlightSinceSimulationStart < GetTimeSinceSimulationStart())
                {
                    flight.FlightState = FlightState.Departed;
                }
            }
        }

        public Action<Flight> FlightLandedEvent { get; set; }
        public Action<Flight> PrepareFlightEvent { get; set; }
    }
}
