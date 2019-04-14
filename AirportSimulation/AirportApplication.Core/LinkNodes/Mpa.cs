namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Mpa : ProcessingNode, IProcessingNode
    {
        public Mpa(ITimerService timerService) : base(timerService)
        {
        }

        public override void ProcessInternal(Baggage baggage)
        {
            //TODO : Implment
            SuccessSuccessor.PassBaggage(baggage);
        }
    }
}
