namespace AirportSimulation.Abstractions.Core.Contracts
{
    using Common.Models;

    public interface IChainLink
    {
        NodeState Status { get; set; }
        ChainLink NextLink { get; set; }
    }
}