namespace AirportSimulation.Core.Contracts.Services
{
    using System.Collections.Generic;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public interface INodeConnectorService
    {
        void ConnectNodes(IList<IChainLink> nodes, IEnumerable<NodeCreationData> nodesData);
    }
}