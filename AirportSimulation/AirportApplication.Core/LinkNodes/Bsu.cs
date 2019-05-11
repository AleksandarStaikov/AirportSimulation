namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using Core.Services;

    public class BSU : ChainLink, IChainLink
    {
        public delegate BSU Factory();

        private Dictionary<string, BaggageBucket> _baggageBuckets;
        private Robot robot;
        
        public BSU(ITimerService timerService) : base(timerService)
        {
            _baggageBuckets = new Dictionary<string, BaggageBucket>();
            robot = new Robot(timerService);
        }

        public override string Destination => this.GetType().Name;

        private void assignBucket(Baggage baggage)
        {
            if (!_baggageBuckets.Keys.Contains(baggage.Flight.FlightNumber))
            {
                addBaggageBucket(baggage.Flight);
            }

            baggage.Destination = baggage.Flight.FlightNumber;
        }

        private void addBaggageBucket(Flight flight) //Do this on flightScheduled
        {
            double timeUntilLoading = (flight.TimeToFlightSinceSimulationStart - TimerService.GetTimeSinceSimulationStart()).TotalMilliseconds;
            timeUntilLoading = timeUntilLoading == 0 ? 0 : timeUntilLoading;

            BaggageBucket temp = new BaggageBucket(flight.FlightNumber, timeUntilLoading, TimerService);
            temp.SetSuccessor(robot);
            temp.timeToLoad += onTimeToLoad;
            robot.AddSuccessor(temp);

            _baggageBuckets.Add(temp.FlightNumber, temp);
        }

        private void onTimeToLoad(string flightNumber)
        {
            foreach(Baggage b in _baggageBuckets[flightNumber].Baggages)
            {
                b.Destination = NextLink.Destination;
            }

            NextLink.OnStatusChangedToFree += _baggageBuckets[flightNumber].DistributeBaggage;
        }

        public override void PassBaggage(Baggage baggage)
        {
            Status = NodeState.Busy;
            if (baggage.Destination == Destination)
            {
                assignBucket(baggage);
            }

            if (robot.Status == NodeState.Free)
            {
                robot.PassBaggage(baggage);
            }
            else
            {
                robot.OnStatusChangedToFree += () =>
                {
                    PassBaggage(baggage);
                };
            }

            Status = NodeState.Free;
        }

        public void SetSuccessor(IChainLink nextLink)
        {
            NextLink = nextLink;

            robot.AddSuccessor(this.NextLink);
        }
    }


}
