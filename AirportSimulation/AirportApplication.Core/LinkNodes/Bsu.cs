namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class BSU : ChainLink, IChainLink
    {
        public delegate BSU Factory(string nodeId);

        private readonly Dictionary<string, BaggageBucket> _baggageBuckets;
        private readonly Robot _inboundRobot;
        private readonly Robot _outboundRobot;

        public BSU(string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
            _baggageBuckets = new Dictionary<string, BaggageBucket>();
            _inboundRobot = new Robot(Guid.NewGuid().ToString(), timerService);
            _outboundRobot = new Robot(Guid.NewGuid().ToString(), timerService);

            TimerService.PrepareFlightEvent += f => OnTimeToLoad(f.FlightNumber);
        }

        public override string Destination => GetType().Name;

        public override void PassBaggage(Baggage baggage)
        {
            Status = NodeState.Busy;
            AddTransportationLog(baggage);

            if (baggage.Destination == Destination)
            {
                AssignBucket(baggage);
                if(baggage.Flight.FlightState == FlightState.InPreparation && _baggageBuckets[baggage.Flight.FlightNumber].isDistributing)
                {
                    OnTimeToLoad(baggage.Flight.FlightNumber);
                }
            }

            if (_inboundRobot.Status == NodeState.Free)
            {
                PassToRobot(baggage);
            }
            else
            {
                _inboundRobot.OnStatusChangedToFree += () => { PassToRobot(baggage); };
            }

        }

        private void PassToRobot(Baggage baggage)
        {
            _inboundRobot.OnStatusChangedToFree = null;
            _inboundRobot.PassBaggage(baggage);
            Status = NodeState.Free;
        }

        private void AssignBucket(Baggage baggage)
        {
            if (!_baggageBuckets.Keys.Contains(baggage.Flight.FlightNumber))
            {
                AddBaggageBucket(baggage.Flight);
            }

            baggage.Destination = baggage.Flight.FlightNumber;
        }

        private void AddBaggageBucket(Flight flight) //TODO: Do this on flightScheduled
        {
            var bucket = new BaggageBucket(flight.FlightNumber, Guid.NewGuid().ToString(), TimerService);
            bucket.SetSuccessor(_outboundRobot);
            _inboundRobot.AddSuccessor(bucket);

            _baggageBuckets.Add(bucket.FlightNumber, bucket);
        }

        private void OnTimeToLoad(string flightNumber)
        {
            if (_baggageBuckets.ContainsKey(flightNumber))
            {
                var _currentBucket = _baggageBuckets[flightNumber];

                NextLink.OnStatusChangedToFree += _currentBucket.DistributeBaggage;
                _currentBucket.isDistributing = true;
            }
        }

        public void SetSuccessor(IChainLink nextLink)
        {
            NextLink = nextLink;

            _outboundRobot.AddSuccessor(NextLink);
        }

        private void AddTransportationLog(Baggage baggage)
        {
            if (baggage.TransportationStartTime != null)
            {
                var transportationStart = baggage.TransportationStartTime ?? 0;
                var transportingTimeElapsed = TimerService.GetTicksSinceSimulationStart() - transportationStart;
                baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                    new TimeSpan(transportingTimeElapsed),
                    string.Format(LoggingConstants.BagReceivedInTemplate, Destination, baggage.TransporterId) + " Transportation time");
                baggage.TransportationStartTime = null;
            }
        }
    }
}