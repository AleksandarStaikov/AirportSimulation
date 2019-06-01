namespace AirportSimulation.Test
{
    using System;
    using Abstractions.Contracts;
    using Common.Models;
    using Core.LinkNodes;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ChainLinkNodeTests
    {
        [Test]
        public void ChainLinkNode_ShouldAdd_ProcessingLog()
        {
            var timerMock = new Mock<ITimerService>();
            timerMock.Setup(time => time.SimulationMultiplier).Returns(1);
            var conveyorMock = new Mock<OneToOneConveyor>(10, Guid.NewGuid().ToString(), timerMock.Object);
            conveyorMock.SetupGet(conv => conv.Destination).Returns("Psc");
            var bag = new Baggage();

            var node = new CheckInDesk(Guid.NewGuid().ToString(), timerMock.Object);
            node.AddSuccessor(conveyorMock.Object);

            node.PassBaggage(bag);

            bag.Log.Count.ShouldBe(1);
        }
    }
}
