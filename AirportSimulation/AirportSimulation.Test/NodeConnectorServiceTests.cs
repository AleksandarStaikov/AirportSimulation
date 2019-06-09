namespace AirportSimulation.Test
{
    using System;
    using System.Collections.Generic;
    using Abstractions.Contracts;
    using Abstractions.Core.Contracts;
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using Autofac.Extras.Moq;
    using Common;
    using Common.Models;
    using Core.Contracts.Services;
    using Core.LinkNodes;
    using Core.LinkNodes.Contracts;
    using Core.Services;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

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
    }
}
