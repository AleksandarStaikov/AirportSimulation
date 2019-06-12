namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IAa : IProcessingNode, IChainLink
    {
        int DropOffNumber { get; }
        string Destination { get; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void Process(IBaggage baggage);
        void PassBaggage(IBaggage baggage);
    }
}