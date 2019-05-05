namespace AirportSimulation.Abstractions.Core
{
    using System.Timers;
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;
    using CuttingEdge.Conditions;

    public abstract class TransportingNode : ChainLink, ITransportingNode
    {
        private const int ConveyorDefaultMovingTime = 1000;

        protected readonly int _length;
        protected Baggage[] _conveyorBelt;
        protected Timer _timer;

        protected TransportingNode(int length, ITimerService timerService)
            : base(timerService)
        {
            _length = length;
            _conveyorBelt = new Baggage[_length];

            _timer = new Timer();
            _timer.Elapsed += (sender, args) => Move();
        }

        protected int LastIndex => _length - 1;
        protected Baggage LastBaggage => _conveyorBelt[LastIndex];

        protected bool HasLastItem => LastBaggage != null;

        public void Start()
        {
            _timer.Interval = ConveyorDefaultMovingTime / TimerService.SimulationMultiplier;
            if (!_timer.Enabled) _timer.Start();
        }

        public void Stop()
        {
            if (_timer.Enabled) _timer.Stop();
        }

        public void SetSuccessor(IChainLink nextLink)
        {
            NextLink = nextLink;
        }

        public override string Destination => NextLink.Destination;

        protected bool CanMove()
        {
            if (NextLink.Status == NodeState.Free) return true;

            if (!HasLastItem) return true;

            return false;
        }

        protected abstract void Move();

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
    }
}