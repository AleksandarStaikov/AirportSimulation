namespace AirportSimulation.Abstractions.Core
{
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;
    using CuttingEdge.Conditions;
    using System.Timers;

    public abstract class TransportingNode : ChainLink, ITransportingNode
    {
        private const int ConveyorDefaultMovingTime = 1000;

        protected int _lastIndex => _length - 1;
        protected readonly int _length;
        protected Baggage[] _conveyorBelt;
        protected Timer _timer;

        public Baggage LastBaggage => _conveyorBelt[_lastIndex];

        public IChainLink NextNode { get; set; }

        public void Start()
        {
            _timer.Interval = ConveyorDefaultMovingTime / TimerService.SimulationMultiplier;
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
        }

        protected TransportingNode(int length, ITimerService timerService) 
            : base(timerService)
        {
            _length = length;
            _conveyorBelt = new Baggage[_length];

            _timer = new Timer();
            _timer.Elapsed += (sender, args) => Move();
        }

        #region Inserting

        protected bool CanAdd(int index = 0)
        {
            return _conveyorBelt[index] == null;
        }

        protected void Add(Baggage baggage, int index = 0)
        {
            Condition.Requires(CanAdd(), "conveyor")
                .IsEqualTo(true, "Trying to add to {0}, while full.");

            _conveyorBelt[index] = baggage;
        }

        #endregion

        private bool HasLastItem() => LastBaggage != null;

        protected bool CanMove()
        {
            if (NextLink.Status == NodeState.Free)
            {
                return true;
            }

            if (!HasLastItem())
            {
                return true;
            }

            return false;
        }

        protected abstract void Move();
    }
}
