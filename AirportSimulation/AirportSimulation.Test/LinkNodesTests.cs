namespace AirportSimulation.Test
{
    using System;
    using System.Linq;
    using Abstractions.Contracts;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Core.LinkNodes;
    using Core.LinkNodes.Contracts;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Internal;
    using Shouldly;

    [TestFixture]
    public class LinkNodesTests
    {
        [Test]
        public void Asc_ShouldPassBaggageToMpa_WhenFailPercentageIsZero()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var ascSettingsMock = new Mock<IAscSettings>();
            ascSettingsMock.Setup(stt => stt.AscInvalidationPercentage).Returns(0);
            var bagMock = new Mock<Baggage>();
            var mpaMock = new Mock<IChainLink>();
            mpaMock.Setup(mpa => mpa.Destination).Returns(typeof(Mpa).Name);

            var asc = new Asc(ascSettingsMock.Object, Guid.NewGuid().ToString(), timerServiceMock.Object);
            asc.AddSuccessor(mpaMock.Object);

            asc.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(Mpa).Name);
        }

        [Test]
        public void Asc_ShouldPassBaggageToBagCollector_WhenFailPercentageIsHundred()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var ascSettingsMock = new Mock<IAscSettings>();
            ascSettingsMock.Setup(stt => stt.AscInvalidationPercentage).Returns(100);
            var bagMock = new Mock<Baggage>();
            var mpaMock = new Mock<IChainLink>();
            mpaMock.Setup(mpa => mpa.Destination).Returns(typeof(BagCollector).Name);

            var asc = new Asc(ascSettingsMock.Object, Guid.NewGuid().ToString(), timerServiceMock.Object);
            asc.AddSuccessor(mpaMock.Object);

            asc.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(BagCollector).Name);
        }

        [Test]
        public void Pua_ShouldPassTheBaggage_ToBagCollector()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var bagCollectorMock = new Mock<IChainLink>();
            bagCollectorMock.Setup(cl => cl.Destination).Returns(typeof(BagCollector).Name);
            var bagMock = new Mock<Baggage>();

            var pua = new PickUpArea(1, Guid.NewGuid().ToString(), timerServiceMock.Object);
            pua.AddSuccessor(bagCollectorMock.Object);

            pua.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(BagCollector).Name);
        }

        [Test]
        public void Pua_ShouldSetDestinationCorrectly()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var puaIndex = 1;
            var pua = new PickUpArea(puaIndex, Guid.NewGuid().ToString(), timerServiceMock.Object);

            pua.Destination.ShouldBe($"P{puaIndex}");
        }

        [Test]
        public void Psc_ShouldPassBaggageToMpa_WhenFailPercentageIsZero()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var pscSettingsMock = new Mock<IPscSettings>();
            pscSettingsMock.Setup(stt => stt.PscInvalidationPercentage).Returns(0);
            var bagMock = new Mock<Baggage>();
            var mpaMock = new Mock<IChainLink>();
            mpaMock.Setup(mpa => mpa.Destination).Returns(typeof(Mpa).Name);

            var asc = new Psc(pscSettingsMock.Object, Guid.NewGuid().ToString(), timerServiceMock.Object);
            asc.AddSuccessor(mpaMock.Object);

            asc.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(Mpa).Name);
        }

        [Test]
        public void Robot_ShouldPassBaggageToMpa_WhenFailPercentageIsZero()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var bagMock = new Mock<IBaggage>();
            bagMock.Setup(b => b.Destination).Returns(typeof(Mpa).Name);
            var mpaMock = new Mock<IChainLink>();
            mpaMock.Setup(mpa => mpa.Destination).Returns(typeof(Mpa).Name);

            var robot = new Robot(Guid.NewGuid().ToString(), timerServiceMock.Object);
            robot.AddSuccessor(mpaMock.Object);

            robot.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(Mpa).Name);
        }

        [Test]
        public void AA_ShouldPassBaggageToMpa_WhenBagIsForMpa()
        {
            var timerServiceMock = new Mock<ITimerService>();
            var bagMock = new Mock<IBaggage>();
            bagMock.Setup(b => b.Destination).Returns(typeof(Mpa).Name);
            var mpaMock = new Mock<IChainLink>();
            mpaMock.Setup(mpa => mpa.Destination).Returns(typeof(Mpa).Name);

            var aa = new Aa(1, Guid.NewGuid().ToString(), timerServiceMock.Object);
            aa.AddSuccessor(mpaMock.Object);

            aa.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(Mpa).Name);
        }

        [Test]
        public void AA_ShouldPassBaggageToBagCollector_WhenBagIsForBagCollector()
        {
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ts => ts.SimulationMultiplier).Returns(1);

            var flightMock = new Mock<IFlight>();
            flightMock.Setup(b => b.FlightState).Returns(FlightState.InPreparation);

            var bagMock = new Mock<IBaggage>();
            bagMock.Setup(b => b.Destination).Returns(typeof(BagCollector).Name);
            bagMock.Setup(b => b.Flight).Returns(flightMock.Object);

            var bagCollector = new Mock<IChainLink>();
            bagCollector.Setup(bc => bc.Destination).Returns(typeof(BagCollector).Name);

            var aa = new Aa(1, Guid.NewGuid().ToString(), timerServiceMock.Object);
            aa.AddSuccessor(bagCollector.Object);

            aa.PassBaggage(bagMock.Object);

            bagMock.Object.Destination.ShouldBe(typeof(BagCollector).Name);
        }

        [Test]
        public void AA_ShouldPassAddDelayedMessageToLog_WhenBagIsDelayingFlight()
        {
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ts => ts.SimulationMultiplier).Returns(1);

            var flightMock = new Mock<IFlight>();
            flightMock.Setup(b => b.FlightState).Returns(FlightState.Departed);

            var bagMock = new Mock<Baggage>();
            bagMock.Object.Destination = typeof(BagCollector).Name;
            bagMock.Object.Flight = flightMock.Object;

            var bagCollector = new Mock<IChainLink>();
            bagCollector.Setup(bc => bc.Destination).Returns(typeof(BagCollector).Name);

            var aa = new Aa(1, Guid.NewGuid().ToString(), timerServiceMock.Object);
            aa.AddSuccessor(bagCollector.Object);

            aa.PassBaggage(bagMock.Object);

            bagMock.Object.Log.ShouldContain(l => l.Description.Contains(LoggingConstants.BagArrivedLateAtAirportArea));
        }

        [Test]
        public void BagCollector_ShouldReceive_UnlimitedNumberOfBags()
        {
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ts => ts.SimulationMultiplier).Returns(1);

            var bg = new BagCollector(Guid.NewGuid().ToString(), timerServiceMock.Object);

            Should.NotThrow(() =>
            {
                bg.PassBaggage(new Mock<Baggage>().Object);
                bg.PassBaggage(new Mock<Baggage>().Object);
                bg.PassBaggage(new Mock<Baggage>().Object);
                bg.PassBaggage(new Mock<Baggage>().Object);
            });
        }

        [Test]
        public void BagCollector_ShouldReturn_CorrectDestination()
        {
            var timerServiceMock = new Mock<ITimerService>();
            timerServiceMock.Setup(ts => ts.SimulationMultiplier).Returns(1);

            var bg = new BagCollector(Guid.NewGuid().ToString(), timerServiceMock.Object);

            bg.Destination.ShouldContain(typeof(BagCollector).Name);
        }
    }
}
