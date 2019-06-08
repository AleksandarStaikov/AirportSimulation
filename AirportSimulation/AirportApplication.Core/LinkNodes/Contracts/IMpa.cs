namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;
    using Common.Models.Contracts;

    public interface IMpa
    {
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void AddSuccessor(IChainLink successor);
        void PassBaggage(IBaggage baggage);
    }
}