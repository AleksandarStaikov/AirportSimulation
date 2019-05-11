using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Abstractions.Contracts;
using AirportSimulation.Abstractions.Core;
using AirportSimulation.Abstractions.Core.Contracts;
using AirportSimulation.Common.Models;
using System.Timers;

namespace AirportSimulation.Core.LinkNodes
{
    public class BaggageBucket : ChainLink, IChainLink
    {
        public string FlightNumber { get; private set; }
        public Queue<Baggage> Baggages { get; private set; }
        private Timer _timer;

        public Action<string> timeToLoad;

        public BaggageBucket(string flightNumber, double timeUntilLoading, ITimerService timerService) : base(timerService)
        {
            FlightNumber = flightNumber;
            Baggages = new Queue<Baggage>();
            _timer = new Timer(timeUntilLoading);

            _timer.Elapsed += (source, args) => onTimeToLoad();
            _timer.Start();
        }

        public override string Destination => FlightNumber;

        private void onTimeToLoad()
        {
            _timer.Stop();
            timeToLoad(FlightNumber);
        }


        public override void PassBaggage(Baggage baggage)
        {
            Baggages.Enqueue(baggage);
        }

        public void DistributeBaggage() //refine
        {
            if (Baggages.Count > 0)
            {
                if (NextLink.Status == NodeState.Free)
                {
                    NextLink.PassBaggage(Baggages.Dequeue());
                }
            }
        }

        public void SetSuccessor(IChainLink nextLink)
        {
            NextLink = nextLink;
        }
    }
}
