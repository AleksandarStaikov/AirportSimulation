namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using System.Collections.Generic;
    using Common.Models;
	using System.Threading.Tasks;

	public class Mpa : ChainLink, IChainLink
    {
        public delegate Mpa Factory(string nodeId);

        private Dictionary<string, ChainLink> _allSuccessors;
        private Dictionary<string, Queue<Baggage>> _baggageDistributors;

        public Mpa(string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
            _allSuccessors = new Dictionary<string, ChainLink>();
            _baggageDistributors = new Dictionary<string, Queue<Baggage>>();
        }

        public override string Destination => this.GetType().Name;

        public void AddSuccessor(ChainLink successor)
        {
            _allSuccessors[successor.Destination] = successor;
            _baggageDistributors[successor.Destination] = new Queue<Baggage>();
        }

        public override void PassBaggage(Baggage baggage)
        {
            SortToDestinationDistributor(baggage);

            _baggageDistributors[baggage.Destination].Enqueue(baggage);
        }

        public void Start()
        {
            foreach (var destination in _baggageDistributors.Keys)
            {
                Task.Run(() =>
                {
                    DistributeBaggage(destination);
                });
            }
        }

        private void DistributeBaggage(string destination)
        {
            var nextNode = _allSuccessors[destination];

			nextNode.OnStatusChangedToFree += () =>
			{
				if (_baggageDistributors[destination].Count > 0)
				{
					if (nextNode.Status == NodeState.Free)
					{
						nextNode.PassBaggage(_baggageDistributors[destination].Dequeue());
					}
				}
			};
		}

        private void SortToDestinationDistributor(Baggage baggage)
        {
            var timeToFlight = (baggage.Flight.TimeToFlightSinceSimulationStart - TimerService.GetTimeSinceSimulationStart()).TotalMilliseconds;

            if (baggage.Flight.FlightState == FlightState.Landed)
            {
				//baggage.Destination = baggage.Flight.PickUpArea;
				baggage.Destination = baggage.Flight.Gate;
				return;
            }

            if (timeToFlight > baggage.Flight.TimeToFlightSinceSimulationStart.TotalMilliseconds * 0.2) //If timeToFlight is bigger than 1/5 of total timeToFlight //Make customizable?/Calculate?
            {
				//baggage.Destination = typeof(BSU).Name;
				baggage.Destination = baggage.Flight.Gate;
			}
            else
            {
                baggage.Destination = baggage.Flight.Gate;
            }
        }
    }
}
