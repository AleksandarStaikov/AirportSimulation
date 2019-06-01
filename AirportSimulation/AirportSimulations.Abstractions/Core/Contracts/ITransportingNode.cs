namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Abstractions.Contracts;

    public interface ITransportingNode : IChainLink, ISingleSuccessor, IPauseResume
    {
        int Length { get; }

        double MoveTime { get; }
    }
}