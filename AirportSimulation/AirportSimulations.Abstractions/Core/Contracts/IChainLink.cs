namespace AirportSimulation.Abstractions.Core.Contracts
{
    using System;
    using Common.Models;

    public interface IChainLink
    {
        NodeState Status { get; set; }
        string Destination { get; }
        Action OnStatusChangedToFree { get; set; }
        void PassBaggage(Baggage baggage);
    }
}