namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    class Aa : ProcessingNode, IProcessingNode
    {
        public Aa(ITimerService timerService) : base(timerService)
        {
        }

        public override void ProcessInternal(Baggage baggage)
        {
            //TODO : Implment
            throw new System.NotImplementedException();
        }
    }
}
