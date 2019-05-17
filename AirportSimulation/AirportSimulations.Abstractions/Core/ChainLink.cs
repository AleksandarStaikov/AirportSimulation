namespace AirportSimulation.Abstractions.Core
{
    using System;
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;

    public abstract class ChainLink : IChainLink
    {
        private NodeState _status;

        public ChainLink(string nodeId, ITimerService timerService)
        {
            TimerService = timerService;
            _status = NodeState.Free;
            NodeId = nodeId;
        }

        protected ITimerService TimerService { get; }

        protected IChainLink NextLink { get; set; }

        public string NodeId { get; set; }

        public abstract string Destination { get; }

        public NodeState Status
        {
            get => _status;
            set
            {
                _status = value;
                if (value == NodeState.Free) OnStatusChangedToFree?.Invoke();
            }
        }

        public Action OnStatusChangedToFree { get; set; }

        public abstract void PassBaggage(Baggage baggage);
    }
}