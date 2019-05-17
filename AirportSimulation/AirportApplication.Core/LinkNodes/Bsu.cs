namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class BSU : ChainLink, IChainLink
    {
        public delegate BSU Factory(string nodeId);

        private readonly Dictionary<string, BaggageBucket> _baggageBuckets;
        private readonly Robot robot;

        public BSU(string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
            _baggageBuckets = new Dictionary<string, BaggageBucket>();
            robot = new Robot(Guid.NewGuid().ToString(), timerService);
            TimerService.PrepareFlightEvent += f => OnTimeToLoad(f.FlightNumber);
        }

        public override string Destination => GetType().Name;

        public override void PassBaggage(Baggage baggage)
        {
            Status = NodeState.Busy;
            if (baggage.Destination == Destination)
            {
                AssignBucket(baggage);
            }

            if (robot.Status == NodeState.Free)
            {
                robot.PassBaggage(baggage);
            }
            else
            {
                robot.OnStatusChangedToFree += () => { PassBaggage(baggage); };
            }

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
            var timeUntilLoading =
                (flight.TimeToFlightSinceSimulationStart - TimerService.GetTimeSinceSimulationStart())
                .TotalMilliseconds;
            timeUntilLoading = timeUntilLoading == 0 ? 0 : timeUntilLoading;

            var temp = new BaggageBucket(flight.FlightNumber, timeUntilLoading, Guid.NewGuid().ToString(), TimerService);
            temp.SetSuccessor(robot);
            temp.timeToLoad += OnTimeToLoad;
            robot.AddSuccessor(temp);

            _baggageBuckets.Add(temp.FlightNumber, temp);
        }

        private void OnTimeToLoad(string flightNumber)
        {
            foreach (var b in _baggageBuckets[flightNumber].Baggages)
            {
                b.Destination = NextLink.Destination;
            }

            NextLink.OnStatusChangedToFree += _baggageBuckets[flightNumber].DistributeBaggage;
        }

        public void SetSuccessor(IChainLink nextLink)
        {
            NextLink = nextLink;

            robot.AddSuccessor(NextLink);
        }
    }
}