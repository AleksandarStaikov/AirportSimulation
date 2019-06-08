namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IBagCollector
    {
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void PassBaggage(IBaggage baggage);
    }
}