namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using System.Collections.Generic;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface ICheckInDispatcher : IChainLink
    {
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void SetCheckIns(List<ICheckInDesk> checkIns);
        void Start();
        void Stop();
        void PassBaggage(IBaggage baggage);
        void DispatchBaggage(Flight flight);
    }
}