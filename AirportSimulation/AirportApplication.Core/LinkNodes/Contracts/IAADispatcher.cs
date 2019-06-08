namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IAADispatcher
    {
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void SetUpGates(List<Aa> gates);
        void PassBaggage(IBaggage baggage);
    }
}