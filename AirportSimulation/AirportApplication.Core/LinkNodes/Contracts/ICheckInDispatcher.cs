namespace AirportSimulation.Core.LinkNodes.Contracts
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Common.Models.Contracts;

    public interface ICheckInDispatcher
    {
        string Destination { get; }
        string NodeId { get; set; }
        NodeState Status { get; set; }
        Action OnStatusChangedToFree { get; set; }
        void SetCheckIns(List<CheckInDesk> checkIns);
        void Start();
        void Stop();
        void PassBaggage(IBaggage baggage);
        void DispatchBaggage(Flight flight);
    }
}