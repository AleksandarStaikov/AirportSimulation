namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Psc : ProcessingNode, IProcessingNode
    {
        public delegate Psc Factory();

        public Psc(ITimerService timerService) : base(timerService)
        {
        }

        public ChainLink FailSuccessor { get; set; }

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(1000), "Primary security check processing");
            //TODO : Implment 
        }

    }
}
