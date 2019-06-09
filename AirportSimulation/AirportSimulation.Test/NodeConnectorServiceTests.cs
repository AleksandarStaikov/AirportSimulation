namespace AirportSimulation.Test
{
    using Abstractions.Core.Contracts;
    using Autofac;
    using Autofac.Extras.Moq;
    using Common;
    using Common.Models;
    using Core.Contracts.Services;
    using Core.LinkNodes;
    using Core.LinkNodes.Contracts;
    using Core.Services;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class NodeConnectorServiceTests
    {
        [Test]
        public void NodeConnectorService()
        {
            using (var mock = AutoMock.GetLoose((a) =>
            {
                a.RegisterType<ChainLinkFactory>().AsImplementedInterfaces();
                a.RegisterType<ConveyorConnector>().AsImplementedInterfaces();
            }))
            {
                var factoryMock = new Mock<IChainLinkFactory>();
                factoryMock.Setup(f => f.CreateConveyorConnector()).Returns(new Mock<IConveyorConnector>().Object);
                var connector = new NodeConnectorService(factoryMock.Object);

                var checkInDispatcherMock = new Mock<ICheckInDispatcher>();
                var aaDispatcherMock = new Mock<IAADispatcher>();
                var bagCollectorMock = new Mock<IBagCollector>();
                var checkInMock = new Mock<ICheckInDesk>();
                checkInMock.Setup(m => m.NodeId).Returns(Guid.NewGuid().ToString());
                var manyToOneConveyorMock = new Mock<IManyToOneConveyor>();
                manyToOneConveyorMock.Setup(m => m.NodeId).Returns(Guid.NewGuid().ToString());

                var nodeDataConveyor = new NodeCreationData()
                {
                    Id = manyToOneConveyorMock.Object.NodeId,
                    NextNodes = null,
                    Type = BuildingComponentType.ManyToOneConveyor
                };
                var nodeDataCheckIn = new NodeCreationData()
                {
                    Id = checkInMock.Object.NodeId,
                    NextNodes = new Dictionary<NodeCreationData, int?>() { { nodeDataConveyor, 0 } },
                    Type = BuildingComponentType.CheckIn
                };

                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeDataConveyor, nodeDataCheckIn }
                };

                var nodes = new List<IChainLink>()
                {
                    bagCollectorMock.Object,
                    aaDispatcherMock.Object,
                    checkInDispatcherMock.Object,
                    checkInMock.Object,
                    manyToOneConveyorMock.Object
                };

                connector.ConnectNodes(nodes, simulationSettings.Nodes);
            }
        }

        [Test]
        public void NodeConnectorService_ShouldAttachEndNodes()
        {
            using (var mock = AutoMock.GetLoose((a) =>
            {
                a.RegisterType<ChainLinkFactory>().AsImplementedInterfaces();
                a.RegisterType<ConveyorConnector>().AsImplementedInterfaces();
            }))
            {
                var factoryMock = new Mock<IChainLinkFactory>();
                factoryMock.Setup(f => f.CreateConveyorConnector()).Returns(new Mock<IConveyorConnector>().Object);
                var connector = new NodeConnectorService(factoryMock.Object);

                var conveyorId = Guid.NewGuid().ToString();
                var aaId = Guid.NewGuid().ToString();

                var checkInDispatcherMock = new Mock<ICheckInDispatcher>();
                var aaDispatcherMock = new Mock<IAADispatcher>();
                var bagCollectorMock = new Mock<IBagCollector>();
                var conveyorMock = new Mock<IOneToOneConveyor>();
                conveyorMock.Setup(m => m.NodeId).Returns(conveyorId);
                var aaMock = new Mock<IAa>();
                aaMock.Setup(m => m.NodeId).Returns(aaId);
                aaMock.Setup(m => m.AddSuccessor(bagCollectorMock.Object));

                var aa = new NodeCreationData()
                {
                    Id = aaId,
                    NextNodes = null,
                    Type = BuildingComponentType.AA
                };
                var conveyor = new NodeCreationData()
                {
                    Id = conveyorId,
                    NextNodes = new Dictionary<NodeCreationData, int?>() { { aa, 0 } },
                    Type = BuildingComponentType.Conveyor
                };

                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { aa, conveyor }
                };

                var nodes = new List<IChainLink>()
                {
                    bagCollectorMock.Object,
                    aaDispatcherMock.Object,
                    checkInDispatcherMock.Object,
                    conveyorMock.Object,
                    aaMock.Object
                };

                connector.ConnectNodes(nodes, simulationSettings.Nodes);

                aaMock.Verify(m => m.AddSuccessor(bagCollectorMock.Object), Times.Once);
            }
        }
    }
}
