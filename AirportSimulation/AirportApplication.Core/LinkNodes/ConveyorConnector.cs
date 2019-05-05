namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class ConveyorConnector : ChainLink, IConveyorConnector
    {
        public delegate ConveyorConnector Factory();

        private IManyToOneConveyor _nextNode;

        public ConveyorConnector(ITimerService timerService) : base(timerService)
        {
        }

        public NodeState Status { get; set; }

        public override string Destination => _nextNode.Destination;

        public Action OnStatusChangedToFree { get; set; }

        public override void PassBaggage(Baggage baggage)
        {
            _nextNode.PassBaggage(baggage, this);
        }

        public void SetNextNode(IManyToOneConveyor conveyor, int index)
        {
            _nextNode = conveyor;
            _nextNode.AddPredecessor(this, index);
            var action = _nextNode.OnStatusChangedToFree(this);
            action += this.OnStatusChangedToFree;
        }
    }
}
