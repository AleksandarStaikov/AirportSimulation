namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using System.Collections.Generic;

    public class BagCollector : ChainLink
    {
        public delegate BagCollector Factory(string nodeId);

        private readonly List<Baggage> _pickedUpBags;

        public BagCollector(string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
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
