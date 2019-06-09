namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models.Contracts;
    using Contracts;
    using System.Collections.Generic;

    public class BagCollector : ChainLink, IBagCollector
    {
        public delegate BagCollector Factory(string nodeId);

        private readonly List<IBaggage> _pickedUpBags;

        public BagCollector(string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
            _pickedUpBags = new List<IBaggage>();
        }

        public override string Destination => this.GetType().Name;

        public override void PassBaggage(IBaggage baggage)
        {
            _pickedUpBags.Add(baggage);
        }
    }
}
