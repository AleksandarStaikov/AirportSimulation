namespace AirportSimulation.Abstractions.Core.Contracts
{
    using System;
    using Common.Models;

    public interface ITransportingNode: IChainLink
    {
        bool CanAdd();
        void Add(Baggage baggage);
    }
}