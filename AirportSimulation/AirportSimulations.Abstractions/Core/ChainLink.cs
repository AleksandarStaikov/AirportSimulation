namespace AirportSimulation.Abstractions.Core
{
    using Common.Models;
    using Contracts;
    using System;
    using Abstractions.Contracts;

    public abstract class ChainLink : IChainLink
    {
        private readonly ITimerService _timerService;
        private NodeState _status;

        public ChainLink(ITimerService timerService)
        {
            _timerService = timerService;
            _status = NodeState.Free;
        }

        protected ITimerService TimerService => _timerService;

        public NodeState Status
        {
            get => _status;
            set
            {
                _status = value;
                if (value == NodeState.Free)
                {
                    OnStatusChangedToFree?.Invoke();
                }
            }
        }

        public string Name => this.GetType().Name;

        public ChainLink NextLink { get; set; }

        public Action OnStatusChangedToFree { get; set; }

        public abstract void PassBaggage(Baggage baggage);
    }
}
