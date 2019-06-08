namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models.Contracts;

    public interface IProcessingNode : IChainLink, IMultiSuccessor
    {
        void Process(IBaggage baggage);
    }
}