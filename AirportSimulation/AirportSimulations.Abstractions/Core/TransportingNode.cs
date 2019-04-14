namespace AirportSimulation.Abstractions.Core
{
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;
    using CuttingEdge.Conditions;
    using System.Timers;

    public abstract class TransportingNode : ChainLink, ITransportingNode
    {
        protected int _lastIndex => _length - 1;
        protected readonly int _length;
        protected Baggage[] _conveyorBelt;
        protected Timer _timer;

        public Baggage LastBaggage => _conveyorBelt[_lastIndex];

        protected TransportingNode(int length, ITimerService timerService) 
            : base(timerService)
        {
            _length = length;
            _conveyorBelt = new Baggage[_length];
            _timer = new Timer();
            _timer.Elapsed += (sender, args) => Move();
            _timer.Interval = 1000;
            _timer.Start();
        }

        #region Inserting

        public bool CanAdd()
        {
            return _conveyorBelt[0] == null;
        }

        public void Add(Baggage baggage)
        {
            Condition.Requires(CanAdd(), "conveyor")
                .IsEqualTo(true, "Trying to add to {0}, while full.");

            Status = NodeState.Busy;

            _conveyorBelt[0] = baggage;
        }

        public override void PassBaggage(Baggage baggage)
        {
            Add(baggage);
        }

        #endregion

        private bool HasLastItem() => LastBaggage != null;

        private bool CanMove()
        {
            if (SuccessSuccessor.Status == NodeState.Free)
            {
                return true;
            }

            if (!HasLastItem())
            {
                return true;
            }

            return false;
        }

        private void Move()
        {
            _timer.Stop();

            if (CanMove())
            {
                if (LastBaggage != null)
                {
                    SuccessSuccessor.PassBaggage(LastBaggage);
                    _conveyorBelt[_lastIndex] = null;
                }

                for (int i = _lastIndex; i > 0; i--)
                {
                    _conveyorBelt[i] = _conveyorBelt[i - 1];
                    _conveyorBelt[i - 1] = null;
                }

                SuccessSuccessor.OnStatusChangedToFree -= Move;
                Status = NodeState.Free;
                _timer.Start();
            }
            else
            {
                SuccessSuccessor.OnStatusChangedToFree += Move;
            }
        }
    }
}
