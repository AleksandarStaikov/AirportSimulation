namespace AirportSimulation.Abstractions.Core
{
    using Common.Models;
    using CuttingEdge.Conditions;
    using System;
    using System.Timers;

    public abstract class TransportingNode : ChainLink
    {
        protected int _lastIndex => _length - 1;
        protected readonly int _length;
        protected Baggage[] _conveyorBelt;
        protected Timer _timer;

        public TransportingNode(int length)
        {
            _length = length;
            _conveyorBelt = new Baggage[_length];
            _timer = new Timer();
            _timer.Elapsed += Move;
            _timer.Interval = 1000;
            _timer.Start();
        }

        public bool CanAdd()
        {
            return _conveyorBelt[0] == null;
        }

        public void Add(Baggage baggage)
        {
            Condition.Requires(CanAdd(), "conveyor")
                .IsEqualTo(true, "Trying to add to {0}, while full.");

            _conveyorBelt[0] = baggage;
        }

        public bool CanGet()
        {
            return _conveyorBelt[_lastIndex] != null;
        }

        public Baggage Get()
        {
            Condition.Requires(CanGet(), "conveyor")
                .IsEqualTo(true, "Trying to get from {0}, while empty.");

            Baggage baggage = _conveyorBelt[_lastIndex];
            _conveyorBelt[_lastIndex] = null;

            return baggage;
        }

        private bool CanMove() => !CanGet();

        private void Move(object sender, EventArgs e)
        {
            if (CanMove())
            {
                for (int i = _lastIndex - 1; i <= 0; i--)
                {
                    _conveyorBelt[i] = _conveyorBelt[i - 1];
                    _conveyorBelt[i - 1] = null;
                }
            }
        }
    }
}
