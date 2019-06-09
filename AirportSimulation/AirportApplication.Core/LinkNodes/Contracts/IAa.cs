namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IAa : IChainLink
    {
        int DropOffNumber { get; }
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void Process(IBaggage baggage);
        void AddSuccessor(IChainLink chainLink);
        void PassBaggage(IBaggage baggage);
    }
}