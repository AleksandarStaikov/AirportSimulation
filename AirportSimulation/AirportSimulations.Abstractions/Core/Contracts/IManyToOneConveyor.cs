namespace AirportSimulation.Abstractions.Core.Contracts
{
    using System;
    using Abstractions.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public interface IManyToOneConveyor : IChainLink , IPauseResume, ISingleSuccessor
    {
        void AddPredecessor(IConveyorConnector predecessor, int attachedToIndex);

        NodeState Status(IConveyorConnector predecessor);

        Action OnStatusChangedToFree(IConveyorConnector predecessor);

        void PassBaggage(IBaggage baggage, IConveyorConnector predecessor);
    }
}