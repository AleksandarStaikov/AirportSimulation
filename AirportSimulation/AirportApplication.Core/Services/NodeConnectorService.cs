namespace AirportSimulation.Core.Services
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;
    using System.Collections.Generic;
    using System.Linq;
    using LinkNodes.Contracts;

    public class NodeConnectorService : INodeConnectorService
    {
        private readonly IChainLinkFactory _chainLinkFactory;

        public NodeConnectorService(IChainLinkFactory chainLinkFactory)
        {
            _chainLinkFactory = chainLinkFactory;
        }

        public void ConnectNodes(IList<IChainLink> nodes, IEnumerable<NodeCreationData> nodesData)
        {
            var checkInDispatcher = GetNodesOfType<ICheckInDispatcher>(nodes).First();
            var aaDispatcher = GetNodesOfType<IAADispatcher>(nodes).First();
            var bagCollector = GetNodesOfType<IBagCollector>(nodes).First();

            checkInDispatcher.SetCheckIns(GetNodesOfType<ICheckInDesk>(nodes).ToList());
            aaDispatcher.SetUpGates(GetNodesOfType<Aa>(nodes).ToList());

            nodesData = nodesData.OrderBy(x => x.NextNodes.Count);

            foreach (var nodeCreationData in nodesData)
            {
                var currentNode = nodes.FirstOrDefault(n => n.NodeId == nodeCreationData.Id);
                var nextNodes = nodes?.Where(n => nodeCreationData.NextNodes?.Select(nn => nn.Key.Id).Contains(n.NodeId) ?? false) ?? new List<IChainLink>();

                //If current node is end node => attach the bagCollector
                if (currentNode is IPickUpArea || currentNode is IAsc || currentNode is IAa)
                {
                    ((IProcessingNode)currentNode).AddSuccessor(bagCollector);
                }

                //Set the successor nodes
                foreach (var nextNode in nextNodes)
                {
                    if (nextNode is IManyToOneConveyor manyToOneConveyor)
                    {
                        var connector = _chainLinkFactory.CreateConveyorConnector();
                        var index = nodeCreationData.NextNodes[nodeCreationData.NextNodes.Keys.FirstOrDefault(k => k.Id == manyToOneConveyor.NodeId)];

                        if (currentNode is ISingleSuccessor single)
                        {
                            single.SetSuccessor(connector);
                        }
                        else if (currentNode is IMultiSuccessor multi)
                        {
                            multi.AddSuccessor(connector);
                        }

                        connector.SetNextNode(manyToOneConveyor, index ?? 0);
                    }
                    else
                    {
                        if (currentNode is ISingleSuccessor single)
                        {
                            single.SetSuccessor(nextNode);
                        }
                        else if (currentNode is IMultiSuccessor multi)
                        {
                            multi.AddSuccessor(nextNode);
                        }
                    }
                }
            }
        }

        private IEnumerable<T> GetNodesOfType<T>(IEnumerable<IChainLink> nodes)
        {
            return nodes.OfType<T>();
        }
    }
}