namespace AirportSimulation.Abstractions.Core.Contracts
{
    public interface IMultiSuccessor
    {
        void AddSuccessor(IChainLink successor);
    }
}
