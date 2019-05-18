namespace AirportSimulation.Abstractions.Core.Contracts
{
    public interface ITransportingNode : IChainLink
    {
        int Length { get; }

        double MoveTime { get; }

        void Start();

        void Stop();

        void SetSuccessor(IChainLink nextLink);
    }
}