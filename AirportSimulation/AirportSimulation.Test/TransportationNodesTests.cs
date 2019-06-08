namespace AirportSimulation.Test
{
    using System;
    using System.Threading;
    using Abstractions.Contracts;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Core.LinkNodes;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class TransportationNodesTests
    {
        [Test]
        public void Conveyor_ShouldMoveTheBaggage_WhenStarted()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);
            var pscMock = new Mock<IProcessingNode>();
            pscMock.Setup(psc => psc.Destination).Returns(typeof(Psc).Name);
            var bag = new Mock<Baggage>();

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);
            conveyor.SetSuccessor(pscMock.Object);
            conveyor.Start();
            conveyor.PassBaggage(bag.Object);
            
            Thread.Sleep((int)conveyor.MoveTime * 2);

            Should.NotThrow(() => conveyor.PassBaggage(new Mock<Baggage>().Object));
        }

        [Test]
        public void Conveyor_ShouldNotMove_WhenLastHasItem_AndNextIsBusy()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);
            var pscMock = new Mock<IProcessingNode>();
            pscMock.Setup(psc => psc.Destination).Returns(typeof(Psc).Name);
            pscMock.Setup(psc => psc.Status).Returns(NodeState.Busy);
            var bag = new Mock<Baggage>();

            var conveyor = new OneToOneConveyor(1, conveyorId.ToString(), timerServiceMock.Object);
            conveyor.SetSuccessor(pscMock.Object);
            
            conveyor.Start();
            conveyor.PassBaggage(bag.Object);

            Thread.Sleep((int)conveyor.MoveTime * 2);

            Should.Throw<ArgumentException>(() => conveyor.PassBaggage(new Mock<Baggage>().Object));
        }

        [Test]
        public void Conveyor_ShouldSetTheStatusToBusy_WhenPassedABag()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);
            var bag = new Mock<Baggage>();

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);
            conveyor.PassBaggage(bag.Object);

            conveyor.Status.ShouldBe(NodeState.Busy);
        }

        [Test]
        public void Conveyor_ShouldNotMoveTheBaggage_WhenStartedAndStopped()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);
            var pscMock = new Mock<IProcessingNode>();
            pscMock.Setup(psc => psc.Destination).Returns(typeof(Psc).Name);
            var bag = new Mock<Baggage>();

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);
            conveyor.SetSuccessor(pscMock.Object);
            conveyor.Start();
            conveyor.Stop();
            conveyor.PassBaggage(bag.Object);

            Thread.Sleep((int)conveyor.MoveTime * 2);

            Should.Throw<ArgumentException>(() => conveyor.PassBaggage(new Mock<Baggage>().Object));
        }

        [Test]
        public void Conveyor_ShouldNotThrow_WhenStartedWithProperSettings()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);

            Should.NotThrow(() => { conveyor.Start(); });
        }

        [Test]
        public void Conveyor_ShouldPassBaggageToTheNext_WhenNextIsFree()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);
            var pscMock = new Mock<IProcessingNode>();
            pscMock.Setup(psc => psc.Destination).Returns(typeof(Psc).Name);
            pscMock.Setup(psc => psc.Status).Returns(NodeState.Free);
            var bag = new Mock<Baggage>();

            var conveyor = new OneToOneConveyor(1, conveyorId.ToString(), timerServiceMock.Object);
            conveyor.SetSuccessor(pscMock.Object);
            conveyor.Start();
            conveyor.PassBaggage(bag.Object);

            Thread.Sleep((int)conveyor.MoveTime*3);

            pscMock.Verify(psc => psc.PassBaggage(bag.Object), Times.Once);

            Should.NotThrow(() => conveyor.PassBaggage(new Mock<Baggage>().Object));
        }

        [Test]
        public void Conveyor_ShouldReturn_TheDestinationOfTheNextNode()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ser => ser.SimulationMultiplier).Returns(1);
            var pscMock = new Mock<IProcessingNode>();
            pscMock.Setup(psc => psc.Destination).Returns(typeof(Psc).Name);

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);
            conveyor.SetSuccessor(pscMock.Object);

            conveyor.Destination.ShouldBe(typeof(Psc).Name);
        }

        [Test]
        public void Conveyor_ShouldSetId_Properly()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);

            conveyor.NodeId.ShouldBe(conveyorId.ToString());
        }

        [Test]
        public void Conveyor_ShouldSetLenght_Properly()
        {
            var conveyorId = Guid.NewGuid();
            var conveyorLength = 5;
            var timerServiceMock = new Mock<ITimerService>();

            var conveyor = new OneToOneConveyor(conveyorLength, conveyorId.ToString(), timerServiceMock.Object);

            conveyor.Length.ShouldBe(conveyorLength);
        }

        [Test]
        public void Conveyor_ShouldThrow_WhenPassingTwoBagsWithoutMoving()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(),  timerServiceMock.Object);
            var bag = new Mock<Baggage>();

            conveyor.PassBaggage(bag.Object);
            Should.Throw<ArgumentException>(() => { conveyor.PassBaggage(bag.Object); });
        }

        [Test]
        public void Conveyor_ShouldThrow_WhenStartedWithNullSettings()
        {
            var conveyorId = Guid.NewGuid();
            var timerServiceMock = new Mock<ITimerService>();

            var conveyor = new OneToOneConveyor(5, conveyorId.ToString(), timerServiceMock.Object);

            Should.Throw<ArgumentOutOfRangeException>(() => { conveyor.Start(); });
        }
    }
}
