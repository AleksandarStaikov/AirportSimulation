namespace AirportSimulation.Test
{
    using Autofac.Extras.Moq;
    using Common;
    using Common.Models;
    using Core.LinkNodes;
    using Core.Services;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class ChainLinkFactoryTests
    {
        [Test]
        public void ChainLinkFactory_ShouldReturnCheckIn_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDependency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.CheckIn
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<CheckInDesk>();
            }
        }

        [Test]
        public void ChainLinkFactory_ShouldReturnConveyor_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDependency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.Conveyor
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<OneToOneConveyor>();
            }
        }

        [Test]
        public void ChainLinkFactory_ShouldReturnManyToOneConveyor_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.ManyToOneConveyor
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<ManyToOneConveyor>();
            }
        }

        [Test]
        public void ChainLinkFactory_ShouldReturnPsc_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.PSC
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<Psc>();
            }
        }

        [Test]
        public void ChainLinkFactory_ShouldReturnAsc_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.ASC
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<Asc>();
            }
        }


        [Test]
        public void ChainLinkFactory_ShouldReturnMpa_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.MPA
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<Mpa>();
            }
        }


        [Test]
        public void ChainLinkFactory_ShouldReturnBsu_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.BSU
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<BSU>();
            }
        }


        [Test]
        public void ChainLinkFactory_ShouldReturnAa_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.AA
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<Aa>();
            }
        }


        [Test]
        public void ChainLinkFactory_ShouldReturnPua_Correctly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();
                var nodeData = new NodeCreationData()
                {
                    Id = Guid.NewGuid().ToString(),
                    NextNodes = null,
                    Type = BuildingComponentType.PA
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                var resultNode = sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<PickUpArea>();
            }
        }
    }
}
