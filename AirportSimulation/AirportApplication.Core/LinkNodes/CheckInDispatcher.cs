namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    public class CheckInDispatcher : ChainLink
    {
        private readonly Queue<Baggage> _queuedCustomers;
        private readonly Timer _timer;

        public delegate CheckInDispatcher Factory();

        public CheckInDispatcher(ITimerService timerService) : base(timerService)
        {
            _queuedCustomers = new Queue<Baggage>();
            //TODO : Implement dispatching rate
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, e) => DispatchBaggage();
        }

        public void Start()
        {
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

        public override void PassBaggage(Baggage baggage)
        {
            throw new NotImplementedException();
        }

        public void DispatchBaggage()
        {
            //TODO : Baggage Factory ?
            var baggage = new Baggage();

            if (NextLink.Status == NodeState.Free)
            {
                NextLink.PassBaggage(baggage);
            }
            else
            {
                if (NextLink.OnStatusChangedToFree == null)
                {
                    NextLink.OnStatusChangedToFree += PassQueuedBaggage;
                }
                _queuedCustomers.Enqueue(baggage);
            }
        }

        public void PassQueuedBaggage()
        {
            if (!_queuedCustomers.Any() || NextLink.Status != NodeState.Free)
            {
                return;
            }

            NextLink.PassBaggage(_queuedCustomers.Dequeue());
        }
    }
}
