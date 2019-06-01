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


                var resultNode =  sut.CreateChainLink(nodeData, simulationSettings);

                resultNode.ShouldBeOfType<CheckInDesk>();
            }
        }

        //private ChainLinkFactory InitiateChainLinkFactory()
        //{
        //    var checkInMock = new MockFactory<>();

        //    var factory = new ChainLinkFactory();

        //    return factory;
        //}
    }
}
