namespace AirportSimulation.Core.Services
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;
    using System.Collections.Generic;
    using System.Linq;

    public class NodeConnectorService : INodeConnectorService
    {
        private readonly IChainLinkFactory _chainLinkFactory;

        public NodeConnectorService(IChainLinkFactory chainLinkFactory)
        {
            _chainLinkFactory = chainLinkFactory;
        }

        public void ConnectNodes(IList<IChainLink> nodes, IEnumerable<NodeCreationData> nodesData)
        {
            var checkInDispatcher = GetNodesOfType<CheckInDispatcher>(nodes).First();
            var aaDispatcher = GetNodesOfType<AADispatcher>(nodes).First();
            var bagCollector = GetNodesOfType<BagCollector>(nodes).First();

            checkInDispatcher.SetCheckIns(GetNodesOfType<CheckInDesk>(nodes).ToList());
            aaDispatcher.SetUpGates(GetNodesOfType<Aa>(nodes).ToList());

            nodesData = nodesData.OrderBy(x => x.NextNodes);

            foreach (var nodeCreationData in nodesData)
            {
                var currentNode = nodes.FirstOrDefault(n => n.NodeId == nodeCreationData.Id);
                var nextNodes = nodes.Where(n => nodeCreationData.NextNodes.Select(nn => nn.Key.Id).Contains(n.NodeId));

                //If current node is end node => attach the bagCollector
                if (currentNode is PickUpArea || currentNode is Asc || currentNode is Aa)
                {
                    ((IProcessingNode)currentNode).AddSuccessor(bagCollector);
                }

                //Set the successor nodes
                foreach (var nextNode in nextNodes)
                {
                    if (nextNode is ManyToOneConveyor manyToOneConveyor)
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