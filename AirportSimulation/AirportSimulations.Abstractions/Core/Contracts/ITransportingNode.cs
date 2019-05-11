namespace AirportSimulation.Abstractions.Core.Contracts
{
    public interface ITransportingNode : IChainLink
    {
        void Start();

        void Stop();

        void SetSuccessor(IChainLink nextLink);
    }
}