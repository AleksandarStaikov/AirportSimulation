namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IAsc
    {
        void Process(IBaggage baggage);
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void AddSuccessor(IChainLink chainLink);
        void PassBaggage(IBaggage baggage);
    }
}