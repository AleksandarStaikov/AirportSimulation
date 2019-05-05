namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;

    public interface IProcessingNode
    {
        void Process(Baggage baggage);

        void AddSuccessor(IChainLink successor);
    }
}