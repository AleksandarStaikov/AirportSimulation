namespace AirportSimulation.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Common.Models;
    using Common.Models.Contracts;
    using Core.Services;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class TimerServiceTests
    {
        [Test]
        public void TimerService_ShouldRunNewTimer_OnRunNewTimer()
        {
            var timerService = new TimerService();
            var settingsMock = new Mock<ISimulationSettings>();
            settingsMock.Setup(s => s.IncomingFlights).Returns(new Mock<List<Flight>>().Object);
            settingsMock.Setup(s => s.OutgoingFlights).Returns(new Mock<List<Flight>>().Object);
            settingsMock.Setup(s => s.Multiplier).Returns(1);

            timerService.SetSettings(settingsMock.Object);

            timerService.RunNewTimer();
            Thread.Sleep(2000);
            timerService.Stop();

            timerService.GetTimeSinceSimulationStart().TotalMilliseconds.ShouldBeGreaterThan(2000);

            timerService.RunNewTimer();
            timerService.Stop();
            timerService.GetTimeSinceSimulationStart().TotalMilliseconds.ShouldBeLessThan(2000);
        }

        [Test]
        public void TimerService_ShouldShouldReturnSameTimeSinceStart_FromBothMethods()
        {
            var timerService = new TimerService();
            var settingsMock = new Mock<ISimulationSettings>();
            settingsMock.Setup(s => s.IncomingFlights).Returns(new Mock<List<Flight>>().Object);
            settingsMock.Setup(s => s.OutgoingFlights).Returns(new Mock<List<Flight>>().Object);
            settingsMock.Setup(s => s.Multiplier).Returns(1);

            timerService.SetSettings(settingsMock.Object);

            timerService.Start();
            Thread.Sleep(2000);
            timerService.Stop();

            timerService.GetTicksSinceSimulationStart().ShouldBeInRange(timerService.GetTimeSinceSimulationStart().Ticks - 5, timerService.GetTimeSinceSimulationStart().Ticks + 5);
        }

        [Test]
        public void TimerService_ConvertMillisecondsToTimeSpan_ShouldConvertCorrectly()
        {
            var timerService = new TimerService();
            var settingsMock = new Mock<ISimulationSettings>();
            settingsMock.Setup(s => s.IncomingFlights).Returns(new Mock<List<Flight>>().Object);
            settingsMock.Setup(s => s.OutgoingFlights).Returns(new Mock<List<Flight>>().Object);
            settingsMock.Setup(s => s.Multiplier).Returns(1);

            timerService.SetSettings(settingsMock.Object);

            timerService.ConvertMillisecondsToTimeSpan(1000).TotalSeconds.ShouldBe(1);
        }

        [Test]
        public void TimerService_CheckForIncomingFlights_GetsTriggered()
        {
            var timerService = new TimerService();
            var settingsMock = new Mock<ISimulationSettings>();
            settingsMock.Setup(s => s.IncomingFlights).Returns(new List<Flight>()
            {
                new Flight()
                {
                    TimeToFlightSinceSimulationStart = timerService.ConvertMillisecondsToTimeSpan(1000),
                    FlightState =  FlightState.Incoming
                }
            });
            settingsMock.Setup(s => s.OutgoingFlights).Returns(new List<Flight>());
            settingsMock.Setup(s => s.Multiplier).Returns(1);

            timerService.SetSettings(settingsMock.Object);

            timerService.Start();
            Thread.Sleep(2000);

            settingsMock.Object.IncomingFlights.First().FlightState.ShouldBe(FlightState.Landed);
        }

        [Test]
        public void TimerService_CheckForFlightPreparations_GetsTriggered()
        {
            var timerService = new TimerService();
            var settingsMock = new Mock<ISimulationSettings>();
            settingsMock.Setup(s => s.OutgoingFlights).Returns(new List<Flight>()
            {
                new Flight()
                {
                    TimeToFlightSinceSimulationStart = timerService.ConvertMillisecondsToTimeSpan(1000),
                    TimeToFlightPreparationSinceSimulationStart =  timerService.ConvertMillisecondsToTimeSpan(500),
                    FlightState =  FlightState.WaitingForPreparation,
                }
            });
            settingsMock.Setup(s => s.IncomingFlights).Returns(new List<Flight>());
            settingsMock.Setup(s => s.Multiplier).Returns(1);

            timerService.SetSettings(settingsMock.Object);

            timerService.Start();
            Thread.Sleep(750);

            settingsMock.Object.OutgoingFlights.First().FlightState.ShouldBe(FlightState.InPreparation);
        }
    }
}
