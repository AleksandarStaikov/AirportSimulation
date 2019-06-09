namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;
    using Common.Models.Contracts;
    using System;

    public interface IConveyorConnector : IChainLink
    {
        NodeState Status { get; }

        string Destination { get; }

        Action OnStatusChangedToFree { get; set; }

        void PassBaggage(IBaggage baggage);

        void SetNextNode(IManyToOneConveyor conveyor, int index);
    }
}
