namespace AirportSimulation.Abstractions.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;
    using CuttingEdge.Conditions;

    public abstract class ProcessingNode : ChainLink, IProcessingNode
    {
        protected List<IChainLink> _allSuccessors;
        protected Baggage _currentBaggage;

        protected ProcessingNode(ITimerService timerService) : base(timerService)
        {
            _allSuccessors = new List<IChainLink>();
        }

        public override string Destination => GetType().Name;

        public void AddSuccessor(IChainLink chainLink)
        {
            _allSuccessors.Add(chainLink);
        }

        public abstract void Process(Baggage baggage);

        public override void PassBaggage(Baggage baggage)
        {
            Status = NodeState.Busy;
            _currentBaggage = baggage;
            if (baggage.TransportationStartTime != null)
            {
                var transportationStart = baggage.TransportationStartTime ?? 0;
                var transportingTimeElapsed = TimerService.GetTicksSinceSimulationStart() - transportationStart;
                baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                    new TimeSpan(transportingTimeElapsed),
                    "Received in " + Destination + " Transportation time");
                baggage.TransportationStartTime = null;
            }

            ProcessInternal(baggage);
        }

        private void ProcessInternal(Baggage baggage)
        {
            Process(baggage);

            NextLink = _allSuccessors
                .FirstOrDefault(x => x.Destination == baggage.Destination);

            Condition
                .Requires(NextLink)
                .IsNotNull();

            _currentBaggage.TransportationStartTime = TimerService.GetTicksSinceSimulationStart();

            Move();
        }

        private void Move()
        {
            if (NextLink.Status == NodeState.Free && _currentBaggage != null)
            {
                NextLink.OnStatusChangedToFree -= Move;
                NextLink.PassBaggage(_currentBaggage);
                Status = NodeState.Free;
            }
            else if (NextLink.Status != NodeState.Free)
            {
                NextLink.OnStatusChangedToFree += Move;
            }
        }
    }
}