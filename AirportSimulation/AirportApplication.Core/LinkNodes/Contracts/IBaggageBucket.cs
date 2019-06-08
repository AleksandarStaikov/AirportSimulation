namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using System.Collections.Generic;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IBaggageBucket
    {
        bool isDistributing { get; set; }
        string FlightNumber { get; }
        Queue<IBaggage> Baggages { get; }
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void PassBaggage(IBaggage baggage);
        void DistributeBaggage();
        void SetSuccessor(IChainLink nextLink);
    }
}