namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IOneToOneConveyor : IChainLink
    {
        int Length { get; }
        double MoveTime { get; }
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void PassBaggage(IBaggage baggage);
        void Start();
        void Stop();
        void SetSuccessor(IChainLink nextLink);
    }
}