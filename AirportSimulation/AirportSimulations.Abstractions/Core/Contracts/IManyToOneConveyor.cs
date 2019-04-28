namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;
    using System;

    public interface IManyToOneConveyor : IChainLink
    {
        void AddPredecessor(IConveyorConnector predecessor, int attachedToIndex);

        NodeState Status(IConveyorConnector predecessor);

        Action OnStatusChangedToFree(IConveyorConnector predecessor);

        void PassBaggage(Baggage baggage, IConveyorConnector predecessor);
    }
}