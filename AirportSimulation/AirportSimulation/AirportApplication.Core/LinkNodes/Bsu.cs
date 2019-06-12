namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Models.Contracts;
    using Contracts;

    public class BSU : ChainLink, ISingleSuccessor, IBSU
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

        public override void PassBaggage(IBaggage baggage)
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

        private void PassToRobot(IBaggage baggage)
        {
            _inboundRobot.OnStatusChangedToFree = null;
            _inboundRobot.PassBaggage(baggage);
            Status = NodeState.Free;
        }

        private void AssignBucket(IBaggage baggage)
        {
            if (!_baggageBuckets.Keys.Contains(baggage.Flight.FlightNumber))
            {
                AddBaggageBucket(baggage.Flight);
            }

            baggage.Destination = baggage.Flight.FlightNumber;
        }

        private void AddBaggageBucket(IFlight flight)
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

        private void AddTransportationLog(IBaggage baggage)
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