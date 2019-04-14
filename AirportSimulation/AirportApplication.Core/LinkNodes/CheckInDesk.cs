namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class CheckInDesk : ProcessingNode, IProcessingNode
    {
        public CheckInDesk(ITimerService timerService) : base(timerService)
        {
        }

        public override void ProcessInternal(Baggage baggage)
        {
            //TODO : Implment
            SuccessSuccessor.PassBaggage(_currentBaggage);
        }
    }
}
