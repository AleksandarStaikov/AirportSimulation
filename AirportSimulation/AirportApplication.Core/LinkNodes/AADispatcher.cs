namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;
    using CuttingEdge.Conditions;

    public class AADispatcher : ChainLink, IAADispatcher
    {
        public delegate AADispatcher Factory(IFlightManagement flightManagement, string nodeId);

        private readonly IFlightManagement _flightManagement;

        private Dictionary<IChainLink, Queue<IBaggage>> _gateQueues;
        private List<Aa> _gates;

        public AADispatcher(IFlightManagement flightManagement, string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
            _flightManagement = flightManagement;
            TimerService.FlightLandedEvent += DispatchFlightBaggage;
        }

        public override string Destination { get; }

        public void SetUpGates(List<Aa> gates)
        {
            _gates = gates;
            _gateQueues = new Dictionary<IChainLink, Queue<IBaggage>>();
            foreach (var gate in gates) _gateQueues.Add(gate, new Queue<IBaggage>());
        }

        private IChainLink FindGate(string gateDestination)
        {
            var gate = _gates.FirstOrDefault(g => g.Destination == gateDestination);

            Condition.Requires(gate).IsNotNull();

            return gate;
        }

        private void DispatchFlightBaggage(Flight flight)
        {
            var gate = FindGate(flight.Gate);

            var randomGen = new Random(0);

            foreach (var i in Enumerable.Range(1, flight.BaggageCount))
            {
                var isTrans = randomGen.Next(0, 101) < _flightManagement.TransBaggagePercentage;

                var bag = new Baggage
                {
                    BaggageType = BaggageType.Small,
                    Destination = typeof(Mpa).Name,
                    Flight = flight,
                    Owner = "Someone"
                };

                if (isTrans)
                {
                    var transFlights = _flightManagement
                        .OutgoingFlights
                        .Where(f => f.TimeToFlightSinceSimulationStart > TimerService.GetTimeSinceSimulationStart()
                                    && f.BaggageCount > f.DispatchedBaggageCount)
                        .ToList();

                    var index = randomGen.Next(0, transFlights.Count());
                    var transFlight = transFlights[index];

                    bag.Flight = transFlight;
                    transFlight.DispatchedBaggageCount++;
                }

                PassOrEnqueueBaggage(gate, bag);
            }
        }

        private void PassOrEnqueueBaggage(IChainLink gate, IBaggage bag)
        {
            bag.TransportationStartTime = TimerService.GetTicksSinceSimulationStart();
            bag.TransporterId = "Queue AA";
            if (gate.Status == NodeState.Free)
            {
                gate.PassBaggage(bag);
            }
            else
            {
                if (gate.OnStatusChangedToFree == null)
                    gate.OnStatusChangedToFree += () => { PassQueuedBaggage(gate); };
                _gateQueues[gate].Enqueue(bag);
            }
        }

        private void PassQueuedBaggage(IChainLink gate)
        {
            var queue = _gateQueues[gate];

            if (!queue.Any() || gate.Status != NodeState.Free) return;

            gate.PassBaggage(queue.Dequeue());
        }

        public override void PassBaggage(IBaggage baggage)
        {
            throw new NotImplementedException();
        }
    }
}