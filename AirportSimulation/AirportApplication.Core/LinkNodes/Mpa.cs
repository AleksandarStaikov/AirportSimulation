namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models.Contracts;

    public class Mpa : ChainLink, IMultiSuccessor, IMpa
    {
        public delegate Mpa Factory(string nodeId);

        private Dictionary<string, IChainLink> _allSuccessors;
        private Dictionary<string, Queue<IBaggage>> _baggageDistributors;

        public Mpa(string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
            _allSuccessors = new Dictionary<string, IChainLink>();
            _baggageDistributors = new Dictionary<string, Queue<IBaggage>>();
        }

        public override string Destination => this.GetType().Name;

        public void AddSuccessor(IChainLink successor)
        {
            _allSuccessors[successor.Destination] = successor;
            _baggageDistributors[successor.Destination] = new Queue<IBaggage>();

            Task.Run(() =>
            {
                DistributeBaggage(successor.Destination);
            });
        }

        public override void PassBaggage(IBaggage baggage)
        {

            AddTransportationLog(baggage);

            SortToDestinationDistributor(baggage);

            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(), TimerService.ConvertMillisecondsToTimeSpan(1000),
                "MPA processing. Sorted to Gate " + baggage.Destination + ". Enqueuing for distribution.");

            _baggageDistributors[baggage.Destination].Enqueue(baggage);
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
                            var tempBaggage = _baggageDistributors[destination].Dequeue();

                            if (tempBaggage == null)
                            {
                                return;
                            }

                            tempBaggage.TransportationStartTime = TimerService.GetTicksSinceSimulationStart();

                            nextNode.PassBaggage(tempBaggage);
                        }
                    }
                };
        }
        
        private void SortToDestinationDistributor(IBaggage baggage)
        {
            var timeToFlight = (baggage.Flight.TimeToFlightSinceSimulationStart - TimerService.GetTimeSinceSimulationStart()).TotalMilliseconds;

            if (baggage.Flight.FlightState == FlightState.Landed)
            {
				baggage.Destination = baggage.Flight.PickUpArea;
				
            }
            else if (baggage.Flight.FlightState == FlightState.WaitingForPreparation) //If timeToFlight is bigger than 1/5 of total timeToFlight //Make customizable?/Calculate?
            {
				baggage.Destination = typeof(BSU).Name;
			}
            else if(baggage.Flight.FlightState == FlightState.InPreparation || baggage.Flight.FlightState == FlightState.Departed)
            {
                baggage.Destination = baggage.Flight.Gate;
            }
        }

        private void AddTransportationLog(IBaggage baggage)
        {
            if (baggage.TransportationStartTime != null)
            {
                var transportationStart = baggage.TransportationStartTime ?? 0;
                var transportingTimeElapsed = TimerService.GetTicksSinceSimulationStart() - transportationStart;
                baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                    new TimeSpan(transportingTimeElapsed),
                    string.Format(LoggingConstants.BagReceivedInTemplate, Destination, baggage.TransporterId));
                baggage.TransportationStartTime = null;
            }
        }
    }
}
