namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;

    public class ConveyorConnector : ChainLink, IConveyorConnector
    {
        public delegate ConveyorConnector Factory(string nodeId);

        private IManyToOneConveyor _nextNode;

        public ConveyorConnector(string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
        }

        //TODO : Pardon the fuck, looks weird
        public NodeState Status => _nextNode.Status(this);

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
