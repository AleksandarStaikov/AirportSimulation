namespace AirportSimulation.Abstractions.Core.Contracts
{
    public interface ISingleSuccessor
    {
        void SetSuccessor(IChainLink nextLink);
    }
}
