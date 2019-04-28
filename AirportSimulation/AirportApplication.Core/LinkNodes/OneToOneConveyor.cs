namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;

    public class OneToOneConveyor : TransportingNode
    {
        public delegate OneToOneConveyor Factory(int length);

        public OneToOneConveyor(int length, ITimerService timerService)
            : base(length, timerService)
        {
        }

        public override void PassBaggage(Baggage baggage)
        {
            Status = NodeState.Busy;
            Add(baggage);
        }

        protected override void Move()
        {
            _timer.Stop();

            if (CanMove())
            {
                if (LastBaggage != null)
                {
                    NextLink.PassBaggage(LastBaggage);
                    _conveyorBelt[_lastIndex] = null;
                }

                for (int i = _lastIndex; i > 0; i--)
                {
                    _conveyorBelt[i] = _conveyorBelt[i - 1];
                    _conveyorBelt[i - 1] = null;
                }

                NextLink.OnStatusChangedToFree -= Move;
                Status = NodeState.Free;
                _timer.Start();
            }
            else
            {
                NextLink.OnStatusChangedToFree += Move;
            }
        }
    }
}
