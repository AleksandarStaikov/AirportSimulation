namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class ConveyorConnector : ChainLink, IConveyorConnector
    {
        private IManyToOneConveyor _nextNode;

        public delegate ConveyorConnector Factory();

        public ConveyorConnector(ITimerService timerService) : base(timerService)
        {
        }

        public NodeState Status { get; set; }

        public string Destination => _nextNode.Destination;

        public Action OnStatusChangedToFree { get; set; }

        public void SetNextNode(IManyToOneConveyor conveyor, int index)
        {
            _nextNode = conveyor;
            _nextNode.AddPredecessor(this, index);
            var action = _nextNode.OnStatusChangedToFree(this);
            action += this.OnStatusChangedToFree;
        }

        public override void PassBaggage(Baggage baggage)
        {
            _nextNode.PassBaggage(baggage, this);
        }
    }
}
