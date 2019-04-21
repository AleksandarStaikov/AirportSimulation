namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using Common.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using CuttingEdge.Conditions;

    public class CheckInDispatcher : ChainLink
    {
        private readonly ISimulationSettings _simulationSettings;

        private List<Queue<Baggage>> _checkInQueues;
        private List<Timer> _flightDropOffTimers;
        private List<CheckInDesk> _checkIns;

        public delegate CheckInDispatcher Factory(ISimulationSettings simulationSettings);

        public CheckInDispatcher(ISimulationSettings simulationSettings, ITimerService timerService) : base(timerService)
        {
            _simulationSettings = simulationSettings;
            SetUpQueues();
            SetUpTimers();

        }

        public void SetCheckIns(List<CheckInDesk> checkIns)
        {
            Condition
                .Requires(checkIns.Count)
                .IsEqualTo(_simulationSettings.CheckInStationsCount);

            _checkIns = checkIns;
        }

        public void Start()
        {
            Condition
                .Requires(_checkIns.Count)
                .IsEqualTo(_simulationSettings.CheckInStationsCount);

            foreach (var timer in _flightDropOffTimers)
            {
                if (!timer.Enabled)
                {
                    timer.Start();
                }
            }
        }

        public void Stop()
        {
            foreach (var timer in _flightDropOffTimers)
            {
                if (timer.Enabled)
                {
                    timer.Stop();
                }
            }
        }

        public override void PassBaggage(Baggage baggage)
        {
            throw new NotImplementedException();
        }

        public void DispatchBaggage(Flight flight)
        {
            //TODO : Baggage Factory ?
            var baggage = new Baggage()
            {
                FlightNumber = flight.FlightNumber,
            };

            var index = FindMostSuitableCheckInIndex();
            var checkIn = _checkIns[index];
            var queue = _checkInQueues[index];

            if (checkIn.Status == NodeState.Free)
            {
                checkIn.PassBaggage(baggage);
            }
            else
            {
                if (checkIn.OnStatusChangedToFree == null)
                {
                    checkIn.OnStatusChangedToFree += () => { PassQueuedBaggage(index); };
                }
                queue.Enqueue(baggage);
            }
        }

        private void PassQueuedBaggage(int index)
        {
            var queue = _checkInQueues[index];
            var checkIn = _checkIns[index];

            if (!queue.Any() || checkIn.Status != NodeState.Free)
            {
                return;
            }

            checkIn.PassBaggage(queue.Dequeue());
        }

        private int FindMostSuitableCheckInIndex()
        {
            var chosen = 0;
            var shortestQueue = _checkInQueues[0].Count;

            foreach (var i in Enumerable.Range(0, _checkIns.Count))
            {
                if (_checkIns[i].Status == NodeState.Free)
                {
                    return i;
                }

                if (_checkInQueues[i].Count < shortestQueue)
                {
                    shortestQueue = _checkInQueues[i].Count;
                    chosen = i;
                }

            }

            return chosen;
        }

        private int CalculateDispatchRate(Flight flight)
        {
            var dispatchRate = TimerService.ConvertTimeSpanToMilliseconds(flight.TimeToFlightSinceSimulationStart) / flight.BaggageCount;
            return dispatchRate / _simulationSettings.Multiplier;
        }

        private void SetUpQueues()
        {
            _checkInQueues = new List<Queue<Baggage>>();

            foreach (var checkIn in _simulationSettings.CheckIns)
            {
                _checkInQueues.Add(new Queue<Baggage>());
            }
        }

        private void SetUpTimers()
        {
            _flightDropOffTimers = new List<Timer>();
            foreach (var flight in _simulationSettings.Flights)
            {
                var timer = new Timer { Interval = CalculateDispatchRate(flight) };
                timer.Elapsed += (sender, e) => DispatchBaggage(flight);

                _flightDropOffTimers.Add(timer);
            }
        }
    }
}
