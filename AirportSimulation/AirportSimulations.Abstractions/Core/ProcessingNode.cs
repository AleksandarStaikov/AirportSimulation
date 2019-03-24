namespace AirportSimulation.Abstractions.Core
{
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;

    public abstract class ProcessingNode : ChainLink, IProcessingNode
    {
        protected Baggage _currentBaggage;

        protected ProcessingNode(ITimerService timerService) : base(timerService)
        {
        }
        
        public abstract void ProcessInternal(Baggage baggage);

        public override void PassBaggage(Baggage baggage)
        {
            Status = NodeState.Busy;
            _currentBaggage = baggage;
            Process();
        }

        public void Process()
        {

            if (SuccessSuccessor.Status == NodeState.Free)
            {
                //TODO: Add Helper {Drop-Off, Discard, Airplane, PickUp, BSU} Nodes
                SuccessSuccessor.OnStatusChangedToFree -= Process;
                ProcessInternal(_currentBaggage);
                Status = NodeState.Free;
            }
            else
            {
                SuccessSuccessor.OnStatusChangedToFree += Process;
            }
        }
    }
}
