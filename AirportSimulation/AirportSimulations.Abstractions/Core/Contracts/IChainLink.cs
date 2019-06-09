namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;
    using Common.Models.Contracts;
    using System;

    public interface IChainLink
    {
        string NodeId { get; set; }
        NodeState Status { get; set; }
        string Destination { get; }
        Action OnStatusChangedToFree { get; set; }
        void PassBaggage(IBaggage baggage);
    }
}