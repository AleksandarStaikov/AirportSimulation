namespace AirportSimulation.Abstractions.Core
{
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
            ProcessInternal(baggage);
        }
        
        public void ProcessInternal(Baggage baggage)
        {
            //TODO : //Refactor time management
            baggage.Log.Add(new BaggageEventLog()
            {
                EventTime = _timerService.GetTimeSinceSimulationStart(),
                Description = $"Starting processing at {this.GetType().ToString()}"
            });
            Process(baggage);
            baggage.Log.Add(new BaggageEventLog()
            {
                EventTime = _timerService.GetTimeSinceSimulationStart(),
                Description = $"Finished processing at {this.GetType().ToString()}"
            });
            Move();
        }

        public void Move()
        {
            if (NextLink.Status == NodeState.Free && this._currentBaggage != null)
            {
                //TODO: Add Helper {Drop-Off, Discard, Airplane, PickUp, BSU} Nodes
                NextLink.OnStatusChangedToFree -= Move;
                NextLink.PassBaggage(_currentBaggage);
                Status = NodeState.Free;
            }
            else
            {
                NextLink.OnStatusChangedToFree += Move;
            }
        }
    }
}
