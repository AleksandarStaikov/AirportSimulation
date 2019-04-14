namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;

    public interface IChainLink
    {
        NodeState Status { get; set; }
        ChainLink SuccessSuccessor { get; set; }
        ChainLink Predecessor { get; set; }
    }
}