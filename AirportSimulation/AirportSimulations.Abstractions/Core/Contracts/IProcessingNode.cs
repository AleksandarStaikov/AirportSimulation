namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;

    public interface IProcessingNode : IMultiSuccessor
    {
        void Process(Baggage baggage);
    }
}