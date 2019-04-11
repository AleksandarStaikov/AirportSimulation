namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Mpa : ProcessingNode, IProcessingNode
    {
        public delegate Mpa Factory();
        public Mpa(ITimerService timerService) : base(timerService)
        {
        }

        public override void Process(Baggage baggage)
        {
            //TODO : Implment
        }
    }
}
