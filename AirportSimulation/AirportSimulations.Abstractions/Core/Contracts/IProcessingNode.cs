namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;

    public interface IProcessingNode
    {
        void Process();

        void ProcessInternal(Baggage baggage);
    }
}