namespace AirportSimulation.Abstractions.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;
    using CuttingEdge.Conditions;

    public abstract class ProcessingNode : ChainLink, IProcessingNode
    {
        protected List<IChainLink> _allSuccessors;
        protected IBaggage _currentBaggage;

        protected ProcessingNode(string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
            _allSuccessors = new List<IChainLink>();
        }

        public override string Destination => GetType().Name;

        public void AddSuccessor(IChainLink chainLink)
        {
            _allSuccessors.Add(chainLink);
        }

        public abstract void Process(IBaggage baggage);

        public override void PassBaggage(IBaggage baggage)
        {
            Status = NodeState.Busy;
            _currentBaggage = baggage;
            if (baggage.TransportationStartTime != null)
            {
                var transportationStart = baggage.TransportationStartTime ?? 0;
                var transportingTimeElapsed = TimerService.GetTicksSinceSimulationStart() - transportationStart;
                baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                    new TimeSpan(transportingTimeElapsed),
                    string.Format(LoggingConstants.BagReceivedInTemplate, Destination, baggage.TransporterId));
                baggage.TransportationStartTime = null;
            }

            ProcessInternal(baggage);
        }

        private void ProcessInternal(IBaggage baggage)
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