﻿namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using CuttingEdge.Conditions;

    public class AADispatcher : ChainLink
    {
        public delegate AADispatcher Factory(IFlightManagement flightManagement);

        private readonly IFlightManagement _flightManagement;

        private Dictionary<IChainLink, Queue<Baggage>> _gateQueues;
        private List<IChainLink> _gates;

        public AADispatcher(IFlightManagement flightManagement, ITimerService timerService) : base(timerService)
        {
            _flightManagement = flightManagement;
            TimerService.FlightLandedEvent += DispatchFlightBaggage;
        }

        public override string Destination { get; }

        public void SetUpGates(List<IChainLink> gates)
        {
            _gates = gates;
            _gateQueues = new Dictionary<IChainLink, Queue<Baggage>>();
            foreach (var gate in gates) _gateQueues.Add(gate, new Queue<Baggage>());
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
                                    && f.BaggageCount > f.DispatchedBaggagesCount)
                        .ToList();

                    var index = randomGen.Next(0, transFlights.Count());
                    var transFlight = transFlights[index];

                    bag.Flight = transFlight;
                    transFlight.DispatchedBaggagesCount++;
                }

                PassOrEnqueueBaggage(gate, bag);
            }
        }

        private void PassOrEnqueueBaggage(IChainLink gate, Baggage bag)
        {
            bag.TransportationStartTime = TimerService.GetTicksSinceSimulationStart();
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

        public override void PassBaggage(Baggage baggage)
        {
            throw new NotImplementedException();
        }
    }
}