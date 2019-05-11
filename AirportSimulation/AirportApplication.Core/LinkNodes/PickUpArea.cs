namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;

    public class PickUpArea : ProcessingNode
    {
        private const int DefaultPuckUpTimeInMilliseconds = 800;

        private readonly int _pickUpRate;

        public PickUpArea(ITimerService timerService, int pickUpRate = 0) : base(timerService)
        {
            if (pickUpRate == 0)
            {
                _pickUpRate = DefaultPuckUpTimeInMilliseconds;
            }
            else
            {
                _pickUpRate = pickUpRate;
            }
        }

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(_pickUpRate), "PickUpArea processings");
        }
    }
}
