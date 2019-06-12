namespace AirportSimulation.Core.LinkNodes
{
    using AirportSimulation.Abstractions.Contracts;
    using AirportSimulation.Abstractions.Core;
    using AirportSimulation.Abstractions.Core.Contracts;
    using AirportSimulation.Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class Robot : ProcessingNode, IProcessingNode, IRobot
    {
        public Robot(string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
        }

        public override void Process(IBaggage baggage)
        {
            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                TimerService.ConvertMillisecondsToTimeSpan(1000),
                string.Format(LoggingConstants.ReceivedInRobotSendingTo, (baggage.Destination == typeof(Mpa).Name ? baggage.Destination : $"{typeof(BaggageBucket).Name} #" + baggage.Destination)));
        }
    }
}
