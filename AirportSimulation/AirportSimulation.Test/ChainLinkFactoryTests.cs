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
    using System.Data;
    using Core.Contracts.Services;
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

        [Test]
        public void ChainLinkFactory_CreateAA_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateAa());
            }
        }

        [Test]
        public void ChainLinkFactory_CreatePsc_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreatePsc());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateAsc_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateAsc());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateMpa_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateMpa());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateCheckInDesk_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateCheckInDesk());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateCheckInDispatcher_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateCheckInDispatcher());
            }
        }


        [Test]
        public void ChainLinkFactory_CreateBsu_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateBsu());
            }
        }


        [Test]
        public void ChainLinkFactory_CreateAaDispatcher_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateAaDispatcher());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateBagCollector_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateBagCollector());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateConveyorConnector_ShouldValidateSettings()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // The AutoMock class will inject a mock IDepsendency
                // into the SystemUnderTest constructor
                var sut = mock.Create<ChainLinkFactory>();

                Should.Throw<NoNullAllowedException>(() => sut.CreateConveyorConnector());
            }
        }

        [Test]
        public void ChainLinkFactory_CreateCheckIn_ShouldReturnNodeWithRandomGuid()
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

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateCheckInDesk();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreateAa_ShouldReturnNodeWithRandomGuid()
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
                    Type = BuildingComponentType.AA
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateAa();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreatePsc_ShouldReturnNodeWithRandomGuid()
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
                    Type = BuildingComponentType.PSC
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreatePsc();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreateAsc_ShouldReturnNodeWithRandomGuid()
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
                    Type = BuildingComponentType.ASC
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateAsc();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }

        }

        [Test]
        public void ChainLinkFactory_CreateMpa_ShouldReturnNodeWithRandomGuid()
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
                    Type = BuildingComponentType.MPA
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateMpa();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreateBsu_ShouldReturnNodeWithRandomGuid()
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
                    Type = BuildingComponentType.BSU
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateBsu();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }


        [Test]
        public void ChainLinkFactory_CreateManyToOneConveyor_ShouldReturnNodeWithRandomGuid()
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
                    Type = BuildingComponentType.ManyToOneConveyor
                };
                var simulationSettings = new SimulationSettings()
                {
                    Nodes = new List<NodeCreationData>() { nodeData }
                };

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateManyToOneConveyor(10);

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreateOneToOneConveyor_ShouldReturnNodeWithRandomGuid()
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

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateOneToOneConveyor(10);

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreateConveyorConnector_ShouldReturnNodeWithRandomGuid()
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

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateConveyorConnector();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreatePuckUpArea_ShouldReturnNodeWithRandomGuid()
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

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreatePua();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }

        [Test]
        public void ChainLinkFactory_CreateBagCollector_ShouldReturnNodeWithRandomGuid()
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

                sut.SetSettings(simulationSettings);

                var resultNode = sut.CreateBagCollector();

                Guid.TryParse(resultNode.NodeId, out Guid output).ShouldBe(true);
            }
        }
    }
}
