namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Aa : ProcessingNode, IProcessingNode
    {
        public delegate Aa Factory();

        public Aa(ITimerService timerService) : base(timerService)
        {
        }

        public override void Process(Baggage baggage)
        {
            //TODO : Implment
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(1000), "AA processing");
        }
    }
}
