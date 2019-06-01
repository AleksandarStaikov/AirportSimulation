namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using Common.Models.Contracts;
    using CuttingEdge.Conditions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using Common;

    public class CheckInDispatcher : ChainLink, IPauseResume
    {
        public delegate CheckInDispatcher Factory(ISimulationSettings simulationSettings, string nodeId);

        //                                                          sec * milliseconds  
        private const int DefaultMaxPreflightBufferInMilliseconds = 600 * 1000;

        private readonly ISimulationSettings _simulationSettings;

        private List<Queue<Baggage>> _checkInQueues;
        private List<CheckInDesk> _checkIns;
        private List<Timer> _flightDropOffTimers;

        public CheckInDispatcher(ISimulationSettings simulationSettings,string nodeId,  ITimerService timerService)
            : base(nodeId, timerService)
        {
            _simulationSettings = simulationSettings;
            SetUpQueues();
            SetUpTimers();

        }

        public override string Destination => this.GetType().Name;

        public void SetCheckIns(List<CheckInDesk> checkIns)
        {
            Condition
                .Requires(checkIns.Count)
                .IsEqualTo(_simulationSettings.Nodes.Count(n => n.Type == BuildingComponentType.CheckIn));

            _checkIns = checkIns;
        }

        public void Start()
        {
            Condition
                .Requires(_checkIns.Count)
                .IsEqualTo(_simulationSettings.Nodes.Count(n => n.Type == BuildingComponentType.CheckIn));

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
            flight.DispatchedBaggageCount++;
            var baggage = new Baggage()
            {
                Flight = flight,
            };

            var index = FindMostSuitableCheckInIndex();
            var checkIn = _checkIns[index];
            var queue = _checkInQueues[index];

            baggage.TransportationStartTime = TimerService.GetTicksSinceSimulationStart();
            baggage.TransporterId = "Queue CheckIn";
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
            var millisecondsBetweenStartAndFlight = TimerService.ConvertTimeSpanToMilliseconds(flight.TimeToFlightSinceSimulationStart);
            var calculatedPreflightBuffer = millisecondsBetweenStartAndFlight * 0.1;

            if (calculatedPreflightBuffer > DefaultMaxPreflightBufferInMilliseconds)
            {
                calculatedPreflightBuffer = DefaultMaxPreflightBufferInMilliseconds;
            }

            var timeUntillFlightWithoutPreflightBuffer = millisecondsBetweenStartAndFlight - calculatedPreflightBuffer;

            var dispatchRate = timeUntillFlightWithoutPreflightBuffer / flight.BaggageCount;
            return (int)dispatchRate / _simulationSettings.Multiplier;
        }

        private void SetUpQueues()
        {
            _checkInQueues = new List<Queue<Baggage>>();

            foreach (var checkIn in Enumerable.Range(0, _simulationSettings.Nodes.Count(n => n.Type == BuildingComponentType.CheckIn)))
            {
                _checkInQueues.Add(new Queue<Baggage>());
            }
        }

        private void SetUpTimers()
        {
            _flightDropOffTimers = new List<Timer>();
            foreach (var flight in _simulationSettings.OutgoingFlights)
            {
                var timer = new Timer { Interval = CalculateDispatchRate(flight) };
                timer.Elapsed += (sender, e) =>
                {
                    if (flight.BaggageCount > flight.DispatchedBaggageCount)
                    {
                        DispatchBaggage(flight);
                    }
                    else
                    {
                        timer.Stop();
                    }
                };

                _flightDropOffTimers.Add(timer);
            }
        }
    }
}
