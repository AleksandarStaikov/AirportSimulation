namespace AirportSimulation.Core.LinkNodes
{
    using AirportSimulation.Abstractions.Contracts;
    using AirportSimulation.Abstractions.Core;
    using AirportSimulation.Abstractions.Core.Contracts;
    using AirportSimulation.Common.Models;

    public class Robot : ProcessingNode, IProcessingNode
    {
        public Robot(string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
        }

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(), 
                TimerService.ConvertMillisecondsToTimeSpan(1000), 
                "Robot processing. Sending to " + 
                (baggage.Destination == typeof(Mpa).Name ? baggage.Destination : "BaggageBucket #" + baggage.Destination));
        }
    }
}
