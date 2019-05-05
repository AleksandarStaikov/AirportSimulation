namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Aa : ProcessingNode, IProcessingNode
    {
        public delegate Aa Factory(int dropOffNumber);

        public Aa(int dropOffNumber, ITimerService timerService) : base(timerService)
        {
            DropOffNumber = dropOffNumber;
        }

        public int DropOffNumber { get; }

        public override string Destination => $"Gate {DropOffNumber}";

        public override void Process(Baggage baggage)
        {
            //TODO : Implment
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(1000), $"{Destination} processing");
        }
    }
}