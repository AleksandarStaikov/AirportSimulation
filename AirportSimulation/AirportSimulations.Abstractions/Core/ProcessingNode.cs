namespace AirportSimulation.Abstractions.Core
{
    using System;
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;

    public abstract class ProcessingNode : ChainLink, IProcessingNode
    {
        private readonly ITimerService _timerService;
        protected Baggage _currentBaggage;

        protected ProcessingNode(ITimerService timerService) : base(timerService)
        {
            _timerService = timerService;
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
                baggage.AddEventLog(new TimeSpan(ticks: transportingTimeElapsed), "Received in " + this.GetType().Name + " Transportation time");
                baggage.TransportationStartTime = null;
            }
            ProcessInternal(baggage);
        }
        
        public void ProcessInternal(Baggage baggage)
        {
            Process(baggage);
            _currentBaggage.TransportationStartTime = TimerService.GetTicksSinceSimulationStart();
            Move();
        }

        public void Move()
        {
            if (NextLink.Status == NodeState.Free && this._currentBaggage != null)
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
