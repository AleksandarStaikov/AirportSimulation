namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using System.Collections.Generic;
    using Common.Models;
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    public class Mpa : ChainLink, IChainLink
    {
        public delegate Mpa Factory();

        private Dictionary<string, ChainLink> _allSuccessors;
        private Dictionary<string, Queue<Baggage>> _baggageDistributors;

        public Mpa(ITimerService timerService) : base(timerService)
        {
            _allSuccessors = new Dictionary<string, ChainLink>();
            _baggageDistributors = new Dictionary<string, Queue<Baggage>>();

        }

        public override string Destination => this.GetType().Name;

        public void AddSuccessor(ChainLink successor)
        {
            _allSuccessors[successor.Destination] = successor;
            _baggageDistributors[successor.Destination] = new Queue<Baggage>();
        }

        public override void PassBaggage(Baggage baggage)
        {
            sortToDestinationDistributor(baggage);

            _baggageDistributors[baggage.Destination].Enqueue(baggage);
        }

        public void Start()
        {
            foreach (string destination in _baggageDistributors.Keys)
            {
                Task.Factory.StartNew(() =>
                {
                    distributeBaggage(destination);
                });
            }
        }

        private void distributeBaggage(string destination)
        {
            ChainLink nextNode = _allSuccessors[destination];

            nextNode.OnStatusChangedToFree += () =>
                {
                    if (_baggageDistributors[destination].Count > 0)
                    {
                        if (nextNode.Status == NodeState.Free)
                        {
                            nextNode.PassBaggage(_baggageDistributors[destination].Dequeue());
                        }
                    }
                };

        }

        private void sortToDestinationDistributor(Baggage baggage)
        {
            double timeToFlight = (baggage.Flight.TimeToFlightSinceSimulationStart - TimerService.GetTimeSinceSimulationStart()).TotalMilliseconds;

            if (baggage.Flight.FlightState == FlightState.Landed)
            {
                baggage.Destination = baggage.Flight.PickUpArea;
                return;
            }

            if (timeToFlight > baggage.Flight.TimeToFlightSinceSimulationStart.TotalMilliseconds * 0.2) //If timeToFlight is bigger than 1/5 of total timeToFlight //Make customizable?/Calculate?
            {
                baggage.Destination = typeof(BSU).Name;
            }
            else
            {
                baggage.Destination = baggage.Flight.Gate;
            }
        }
    }
}
