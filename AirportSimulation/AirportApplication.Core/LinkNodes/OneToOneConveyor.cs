﻿namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class OneToOneConveyor : TransportingNode, IOneToOneConveyor
    {
        public delegate OneToOneConveyor Factory(int length, string nodeId);

        public OneToOneConveyor(int length, string nodeId, ITimerService timerService)
            : base(length, nodeId, timerService)
        {
        }

        public override void PassBaggage(IBaggage baggage)
        {
            Status = NodeState.Busy;
            baggage.TransporterId = NodeId;
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
                    _conveyorBelt[LastIndex] = null;
                }

                for (int i = LastIndex; i > 0; i--)
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
