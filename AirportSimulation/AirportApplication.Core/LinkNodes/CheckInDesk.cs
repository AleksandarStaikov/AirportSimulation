namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class CheckInDesk : ProcessingNode, IProcessingNode
    {
        public delegate CheckInDesk Factory();

        public CheckInDesk(ITimerService timerService) : base(timerService)
        {
        }

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(1000), "CheckIn processing");
            baggage.Destination = typeof(Psc).Name;
        }
    }
}
