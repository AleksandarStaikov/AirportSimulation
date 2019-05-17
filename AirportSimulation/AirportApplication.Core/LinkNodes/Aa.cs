namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Aa : ProcessingNode, IProcessingNode
    {
        public delegate Aa Factory(int dropOffNumber, string nodeId);

        public Aa(int dropOffNumber, string nodeId, ITimerService timerService) : base(nodeId, timerService)
        {
            DropOffNumber = dropOffNumber;
        }

        public int DropOffNumber { get; }

        public override string Destination => $"A{DropOffNumber}";

        public override void Process(Baggage baggage)
        {
            if (baggage.Destination != typeof(Mpa).Name)
            {
                baggage.Destination = typeof(BagCollector).Name;
            }

            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                TimerService.ConvertMillisecondsToTimeSpan(1000),
                $"{Destination} processing");
        }
    }
}