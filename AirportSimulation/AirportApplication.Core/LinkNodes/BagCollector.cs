namespace AirportSimulation.Core.LinkNodes
{
    using System.Collections.Generic;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;

    public class BagCollector : ChainLink
    {
        public delegate BagCollector Factory();

        private readonly List<Baggage> _pickedUpBags;

        public BagCollector(ITimerService timerService) : base(timerService)
        {
            _pickedUpBags = new List<Baggage>();
        }

        public override string Destination => this.GetType().Name;

        public override void PassBaggage(Baggage baggage)
        {
            _pickedUpBags.Add(baggage);
        }
    }
}
