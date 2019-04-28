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

        private ManyToManyConveyor _mainConveyor;
        private Sorter _sorter; //PoC implementation
        public new List<ChainLink> NextLink { get; set; } //[0] is BSU, [1] is MpaToAA //PoC implementation

        public Mpa(ITimerService timerService) : base(timerService)
        {
            NextLink = new List<ChainLink>();
            _mainConveyor = new ManyToManyConveyor(10, timerService);
            _mainConveyor.Start();
            _sorter = new Sorter(timerService, this);

            _mainConveyor.NextLink = _mainConveyor;
        }

        public override void PassBaggage(Baggage baggage)
        {
            Action statusIsFree = () =>
            {
                this.PassBaggage(baggage);
            };

            this.Status = NodeState.Busy;
            if (_mainConveyor.Status == NodeState.Free)
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

        public void AddConnection(int index, ChainLink nextLink) //PoC Implementation
        {
            _mainConveyor.connections.Add(index, nextLink);
        }

        internal class Sorter : ProcessingNode, IProcessingNode //PoC Implementation
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

        internal class ManyToManyConveyor : TransportingNode, ITransportingNode //TESTING
        {
            //protected new List<ChainLink> _conveyorBelt;
            public Dictionary<int, ChainLink> connections;

            public ManyToManyConveyor(int length, ITimerService timerService) : base(length, timerService)
            {
                connections = new Dictionary<int, ChainLink>();

                //_conveyorBelt = new List<ChainLink>(length);
                //connectChain();
            }

            //private void connectChain()
            //{
            //    for(int i = 0; i < _conveyorBelt.Count - 1; i++)
            //    {
            //        _conveyorBelt[0].NextLink = _conveyorBelt[i + 1];
            //    }
            //}

            public override void PassBaggage(Baggage baggage)
            {
                base.PassBaggage(baggage);
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

                        if (connections.ContainsKey(i) && _conveyorBelt[i] != null)
                        {
                            if (((Aa)connections[i].NextLink).CurrentFlight == _conveyorBelt[i].FlightNumber)
                            {
                                if (connections[i].NextLink.Status == NodeState.Free)
                                {
                                    connections[i].NextLink.PassBaggage(_conveyorBelt[i]);
                                    _conveyorBelt[i] = null;
                                }
                                else
                                {
                                    _conveyorBelt[i] = _conveyorBelt[i - 1];
                                    _conveyorBelt[i - 1] = null;
                                }
                            }
                        }
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
}
