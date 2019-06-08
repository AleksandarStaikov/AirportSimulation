namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class CheckInDesk : ProcessingNode, IProcessingNode, ICheckInDesk
    {
        public delegate CheckInDesk Factory(string nodeId);

        public CheckInDesk(string nodeId, ITimerService timerService) 
            : base(nodeId, timerService)
        {
        }

        public override void Process(IBaggage baggage)
        {
            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(), 
                TimerService.ConvertMillisecondsToTimeSpan(1000),
                "CheckIn processing");
            baggage.Destination = typeof(Psc).Name;
        }
    }
}
