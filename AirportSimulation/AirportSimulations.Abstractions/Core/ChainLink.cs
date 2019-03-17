namespace AirportSimulation.Abstractions.Core
{
    using Common.Models;
    using Contracts;
    using System;

    public abstract class ChainLink : IChainLink
    {
        private NodeState _status;

        public ChainLink()
        {
            _status = NodeState.Free;
        }

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
        
        public ChainLink SuccessSuccessor { get; set; }

        public ChainLink Predecessor { get; set; }

        public Action OnStatusChangedToFree { get; set; }

        public abstract void PassBaggage(Baggage baggage);
    }
}
