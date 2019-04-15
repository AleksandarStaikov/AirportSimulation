namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using System.Collections.Generic;
    using Common.Models;
    using System;

    public class Mpa : ChainLink, IChainLink
    {
        public delegate Mpa Factory();

        private Conveyor _mainConveyor;
        private Sorter _sorter;
        public new List<ChainLink> NextLink { get; set; } //[0] is BSU, [1] is MpaToAA

        public Mpa(ITimerService timerService) : base(timerService)
        {
            NextLink = new List<ChainLink>();
            _mainConveyor = new Conveyor(10, timerService);
            _mainConveyor.Start();
            _sorter = new Sorter(timerService, this);

            _mainConveyor.NextLink = _sorter;
        }

        public override void PassBaggage(Baggage baggage)
        {
            Action statusIsFree = () =>
            {
                this.PassBaggage(baggage);
            };
            
            this.Status = NodeState.Busy;
            if(_mainConveyor.Status == NodeState.Free)
            {
                _mainConveyor.PassBaggage(baggage);
                this.Status = NodeState.Free;
                _mainConveyor.OnStatusChangedToFree -= statusIsFree;
                
            }
            else
            {
                _mainConveyor.OnStatusChangedToFree += statusIsFree;
            }
        }

        internal class Sorter : ProcessingNode, IProcessingNode
        {
            private Mpa _mpa;

            public Sorter(ITimerService timerService, Mpa mpa) : base(timerService)
            {
                _mpa = mpa;
            }

            public override void Process(Baggage baggage)
            {
                if ((SimulationSettings.TimeToFlight.TotalMilliseconds - TimerService.GetTimeSinceSimulationStart().TotalMilliseconds * TimerService.SimulationMultiplier) > 30000)
                {
                    this.NextLink = _mpa.NextLink[0];
                }
                else
                {
                    this.NextLink = _mpa.NextLink[1];
                }
            }
        }
    }
}
