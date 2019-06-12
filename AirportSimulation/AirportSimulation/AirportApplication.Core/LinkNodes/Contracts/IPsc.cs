namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;
    using Common.Models.Contracts;

    public interface IPsc : IProcessingNode
    {
        void Process(IBaggage baggage);
        string Destination { get; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void AddSuccessor(IChainLink chainLink);
        void PassBaggage(IBaggage baggage);
    }
}