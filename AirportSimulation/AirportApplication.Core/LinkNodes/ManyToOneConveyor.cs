﻿namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using Common.Models.Contracts;

    public class ManyToOneConveyor : TransportingNode, IManyToOneConveyor
    {
        public delegate ManyToOneConveyor Factory(int length, string nodeId);

        private readonly Dictionary<IConveyorConnector, int> _incomingConnections;
        private readonly Dictionary<int, Action> _statusChangedToFreeEvents;

        public ManyToOneConveyor(int length, string nodeId, ITimerService timerService)
            : base(length, nodeId, timerService)
        {
            _incomingConnections = new Dictionary<IConveyorConnector, int>();
            _statusChangedToFreeEvents = new Dictionary<int, Action>();
        }

        public void AddPredecessor(IConveyorConnector predecessor, int attachedToIndex)
        {
            _incomingConnections.Add(predecessor, attachedToIndex);
            _statusChangedToFreeEvents.Add(attachedToIndex, null);
        }

        public NodeState Status(IConveyorConnector predecessor)
        {
            var index = _incomingConnections[predecessor];
            return _conveyorBelt[index] == null ? NodeState.Free : NodeState.Busy;
        }

        public Action OnStatusChangedToFree(IConveyorConnector predecessor)
        {
            var index = _incomingConnections[predecessor];
            return _statusChangedToFreeEvents[index];
        }

        public void PassBaggage(IBaggage baggage, IConveyorConnector predecessor)
        {
            var index = _incomingConnections[predecessor];
            if (CanAdd(index))
            {
                baggage.TransporterId = NodeId;
                Add(baggage, index);
            }
        }

        public override void PassBaggage(IBaggage baggage)
        {
            throw new NotImplementedException();
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
                foreach (var kvp in _statusChangedToFreeEvents)
                {
                    if (_conveyorBelt[kvp.Key] == null)
                    {
                        kvp.Value?.Invoke();
                    }
                }

                _timer.Start();
            }
            else
            {
                NextLink.OnStatusChangedToFree += Move;
            }
        }
    }
}