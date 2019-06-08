namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IBSU
    {
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void PassBaggage(IBaggage baggage);
        void SetSuccessor(IChainLink nextLink);
    }
}