﻿namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class Aa : ProcessingNode, IProcessingNode, IAa
    {
        public delegate Aa Factory(int dropOffNumber, string nodeId);

        public Aa(int dropOffNumber, string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
            DropOffNumber = dropOffNumber;
        }

        public int DropOffNumber { get; }

        public override string Destination => $"A{DropOffNumber}";

        public override void Process(IBaggage baggage)
        {
            var logMessage = $"{Destination} processing - ";

            if (baggage.Flight.FlightState == FlightState.Landed)
            {
                baggage.Destination = typeof(BagCollector).Name;
                if (baggage.Flight.FlightState == FlightState.Departed)
                {
                    double delayInMinutes =
                        (TimerService.GetTimeSinceSimulationStart() - baggage.Flight.TimeToFlightSinceSimulationStart)
                        .TotalMinutes;
                    logMessage += LoggingConstants.BagArrivedLateAtAirportArea + $" with {delayInMinutes:F2} minutes";
                    baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                        TimerService.ConvertMillisecondsToTimeSpan(1000), logMessage, delayInMinutes);
                }
                else
                {
                    logMessage += LoggingConstants.BagArrivedOnTimeAtAirportArea;
                    baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                        TimerService.ConvertMillisecondsToTimeSpan(1000), logMessage);
                }
            }
            else
            {
                logMessage += LoggingConstants.BagRedirectedToAnotherFlight;

                baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                    TimerService.ConvertMillisecondsToTimeSpan(1000), logMessage);
            }


        }
    }
}