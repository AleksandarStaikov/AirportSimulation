namespace AirportSimulation.Test
{
    using System;
    using System.Linq;
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
        public void ChainLinkNode_ShouldAddProcessingLog()
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

        [Test]
        public void ChainLinkNode_ShouldAddTransportingLog_WhenTransportationTImeIsAvailable()
        {
            var timerMock = new Mock<ITimerService>();
            timerMock.Setup(time => time.SimulationMultiplier).Returns(1);
            var conveyorMock = new Mock<OneToOneConveyor>(10, Guid.NewGuid().ToString(), timerMock.Object);
            conveyorMock.SetupGet(conv => conv.Destination).Returns("Psc");
            var bag = new Baggage();
            bag.TransportationStartTime = 0;

            var node = new CheckInDesk(Guid.NewGuid().ToString(), timerMock.Object);
            node.AddSuccessor(conveyorMock.Object);

            node.PassBaggage(bag);

            bag.Log.Count(l => l.Description.Contains(LoggingConstants.BagTransporterIdText)).ShouldBe(1);
        }

        [Test]
        public void ChainLinkNode_ShouldNotAddTransportingLog_WhenTransportationTImeIsNull()
        {
            var timerMock = new Mock<ITimerService>();
            timerMock.Setup(time => time.SimulationMultiplier).Returns(1);
            var conveyorMock = new Mock<OneToOneConveyor>(10, Guid.NewGuid().ToString(), timerMock.Object);
            conveyorMock.SetupGet(conv => conv.Destination).Returns("Psc");
            var bag = new Baggage();
            //bag.TransportationStartTime = null;

            var node = new CheckInDesk(Guid.NewGuid().ToString(), timerMock.Object);
            node.AddSuccessor(conveyorMock.Object);

            node.PassBaggage(bag);

            bag.Log.Count(l => l.Description.Contains(LoggingConstants.BagTransporterIdText)).ShouldBe(0);
        }

        [Test]
        public void ChainLinkNode_ShouldMove_WhenNextIsFree_AndHasBaggage()
        {
            var timerMock = new Mock<ITimerService>();
            timerMock.Setup(time => time.SimulationMultiplier).Returns(1);
            var conveyorMock = new Mock<OneToOneConveyor>(10, Guid.NewGuid().ToString(), timerMock.Object);
            conveyorMock.SetupGet(conv => conv.Destination).Returns("Psc");
            conveyorMock.SetupGet(conv => conv.Status).Returns(NodeState.Free);
            var bag = new Baggage();

            var node = new CheckInDesk(Guid.NewGuid().ToString(), timerMock.Object);
            node.AddSuccessor(conveyorMock.Object);

            node.PassBaggage(bag);

            conveyorMock.Verify(conv => conv.PassBaggage(bag), Times.Once);
        }
    }
}
