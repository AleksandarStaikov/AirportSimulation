namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using System.Collections.Generic;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class BaggageBucket : ChainLink, IChainLink, IBaggageBucket
    {
        public bool isDistributing { get; set; } = false;

        public BaggageBucket(string flightNumber, string nodeId, ITimerService timerService) 
            : base(nodeId, timerService)
        {
            FlightNumber = flightNumber;
            Baggages = new Queue<IBaggage>();
        }

        public string FlightNumber { get; }
        public Queue<IBaggage> Baggages { get; }

        public override string Destination => FlightNumber;

        public override void PassBaggage(IBaggage baggage)
        {
            Baggages.Enqueue(baggage);
        }

        public void DistributeBaggage()
        {
            if (Baggages.Count > 0)
            {
                if (NextLink.Status == NodeState.Free)
                {
                    var _currentBaggage = Baggages.Dequeue();
                    _currentBaggage.Destination = typeof(Mpa).Name;

                    NextLink.PassBaggage(_currentBaggage);
                }
            }
        }

        public void SetSuccessor(IChainLink nextLink)
        {
            NextLink = nextLink;
        }
    }
}