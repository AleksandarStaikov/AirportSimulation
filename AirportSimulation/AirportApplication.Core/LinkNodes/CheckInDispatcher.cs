namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using Common.Models.Contracts;

    public class CheckInDispatcher : ChainLink
    {
        private readonly IFlightManagement _simulationSettings;
        private readonly Queue<Baggage> _queuedCustomers;
        private readonly Timer _timer;

        public delegate CheckInDispatcher Factory(IFlightManagement flightManagementSe);

        public CheckInDispatcher(IFlightManagement simulationSettings, ITimerService timerService) : base(timerService)
        {
            _simulationSettings = simulationSettings;
            _queuedCustomers = new Queue<Baggage>();

            _timer = new Timer();
            SetUpTimer();
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
            var baggage = new Baggage()
            {
                FlightNumber = _simulationSettings.Flights[0].FlightNumber,
            };

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

        private void SetUpTimer()
        {
            _timer.Interval = CalculateDispatchRate();
            _timer.Elapsed += (sender, e) => DispatchBaggage();
        }

        private int CalculateDispatchRate()
        {
            var firstFlight = _simulationSettings.Flights[0];
            var dispatchRate = TimerService.ConvertTimeSpanToMilliseconds(firstFlight.TimeToFlightSinceSimulationStart) / firstFlight.BaggageCount;
            return dispatchRate / _simulationSettings.Multiplier;
        }
    }
}
