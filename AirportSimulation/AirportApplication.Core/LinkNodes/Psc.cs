namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System.Linq;

    public class Psc : ProcessingNode, IProcessingNode
    {
        public delegate Psc Factory();

        public Psc(ITimerService timerService) : base(timerService)
        {
        }

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(1000), "Primary security check processing");
            //TODO : Implment 
            //TODO : Set baggage destination; 
        }

        protected override void DetermineNextLink()
        {
            NextLink = _allSuccessors.FirstOrDefault(x => x.Destination == _currentBaggage.Destination);
        }
    }
}
