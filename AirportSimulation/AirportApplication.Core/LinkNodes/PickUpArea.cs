namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class PickUpArea : ProcessingNode, IPickUpArea
    {
        public delegate PickUpArea Factory(int pickUpAreaIndex, string nodeId, int pickUpRate = 0);

        private const int DefaultPuckUpTimeInMilliseconds = 800;

        private readonly int _pickUpAreaIndex;
        private readonly int _pickUpRate;

        public PickUpArea(int pickUpAreaIndex, string nodeId, ITimerService timerService, int pickUpRate = 0) : base(nodeId, timerService)
        {
            _pickUpAreaIndex = pickUpAreaIndex;
            _pickUpRate = pickUpRate == 0 ? DefaultPuckUpTimeInMilliseconds : pickUpRate;
        }

        public override string Destination => $"P{_pickUpAreaIndex}";

        public override void Process(IBaggage baggage)
        {
            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(), 
                TimerService.ConvertMillisecondsToTimeSpan(_pickUpRate),
                "PickUpArea processings");
            baggage.Destination = typeof(BagCollector).Name;
        }
    }
}