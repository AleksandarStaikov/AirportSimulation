namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;

    public class PickUpArea : ProcessingNode
    {
        public delegate PickUpArea Factory(int pickUpAreaIndex, int pickUpRate = 0);

        private const int DefaultPuckUpTimeInMilliseconds = 800;

        private readonly int _pickUpAreaIndex;
        private readonly int _pickUpRate;

        public PickUpArea(int pickUpAreaIndex, ITimerService timerService, int pickUpRate = 0) : base(timerService)
        {
            _pickUpAreaIndex = pickUpAreaIndex;
            _pickUpRate = pickUpRate == 0 ? DefaultPuckUpTimeInMilliseconds : pickUpRate;
        }

        public override string Destination => $"P{_pickUpAreaIndex}";

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(_pickUpRate), "PickUpArea processings");
            baggage.Destination = typeof(Mpa).Name;
        }
    }
}