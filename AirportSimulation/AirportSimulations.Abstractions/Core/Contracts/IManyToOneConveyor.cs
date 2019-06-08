namespace AirportSimulation.Abstractions.Core.Contracts
{
    using System;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IManyToOneConveyor : IChainLink
    {
        void AddPredecessor(IConveyorConnector predecessor, int attachedToIndex);

        NodeState Status(IConveyorConnector predecessor);

        Action OnStatusChangedToFree(IConveyorConnector predecessor);

        void PassBaggage(IBaggage baggage, IConveyorConnector predecessor);
    }
}