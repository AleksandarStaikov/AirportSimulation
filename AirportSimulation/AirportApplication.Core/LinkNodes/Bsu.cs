namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Timers;
    
    public class BSU : ChainLink, IChainLink
    {
        public delegate BSU Factory();

        internal Dictionary<string, BaggageBucket> _baggageBuckets;
        internal Conveyor _inboundConveyor;
        internal Conveyor _outboundConveyor;
        internal RobotBSU _robot;
        

        public BSU(ITimerService timerService) : base(timerService)
        {
            _baggageBuckets = new Dictionary<string, BaggageBucket>();
            _inboundConveyor = new Conveyor(5, timerService); //TODO variable conveyor capacity
            _outboundConveyor = new Conveyor(5, timerService); //TODO variable conveyor capacity
            _robot = new RobotBSU(timerService, this); //Length is always 1 //Pass BSU as a parameter in order to access internal fields in Robot
        }

        public void Start()
        {
            _inboundConveyor.NextLink = _robot;
            _outboundConveyor.NextLink = this.NextLink;
            _robot.NextLink = _outboundConveyor;


            _inboundConveyor.Start();
            _outboundConveyor.Start();
            _robot.Start();
        }

        private void addBaggageBucket(Baggage baggage)
        {
            if(!_baggageBuckets.ContainsKey(baggage.FlightNumber))
            {
                BaggageBucket newBucket = new BaggageBucket(baggage.FlightNumber, 1, this.TimerService);
                newBucket.NextLink = _robot;
                newBucket.OnTimeToProcess += passToRobot;
                newBucket.OnBucketEmpty += (string flightNumber) => _baggageBuckets.Remove(flightNumber);

                _baggageBuckets.Add(baggage.FlightNumber, newBucket); //TODO Variable timeToFlight
            }
            
        }

        private void passToRobot(string flightNumber)
        {
            BaggageBucket current = _baggageBuckets[flightNumber];

            if(current.NextLink.Status == NodeState.Free && current.Baggages.Count > 0)
            {
                ((RobotBSU)current.NextLink).PassBaggage(current.Baggages.Pop(), RobotStatus.Outbound);

                current.NextLink.OnStatusChangedToFree -= current.passToRobot;
            }
            else if(current.Baggages.Count == 0)
            {
                current.OnBucketEmpty(flightNumber);
            }
            else
            {
                current.NextLink.OnStatusChangedToFree += current.passToRobot;
            }
        }

        public override void PassBaggage(Baggage baggage)
        {
            if (baggage.TransportationStartTime != null)
            {
                var transportationStart = baggage.TransportationStartTime ?? 0;
                var transportingTimeElapsed = TimerService.GetTicksSinceSimulationStart() - transportationStart;
                baggage.AddEventLog(new TimeSpan(ticks: transportingTimeElapsed), "Received in " + this.GetType().Name + " Transportation time");
                baggage.TransportationStartTime = null;
            }
            Action statusIsFree = () =>
            {
                this.PassBaggage(baggage);
            };

            this.Status = NodeState.Busy;
            if (_inboundConveyor.Status == NodeState.Free)
            {
                addBaggageBucket(baggage);
                _inboundConveyor.PassBaggage(baggage);
                this.Status = NodeState.Free;
                _inboundConveyor.OnStatusChangedToFree -= statusIsFree;

            }
            else
            {
                _inboundConveyor.OnStatusChangedToFree += statusIsFree;
            }
        }

        #region BaggageBucket

        //Possibly externalize this class. No need so far, as only used in BSU
        internal class BaggageBucket : ChainLink, IChainLink
        {
            private const int DEFAULT_MOVING_TIME = 1000;

            private string _flightNumber;
            private int _timeToFLight;
            private Timer _timer;
            public Stack<Baggage> Baggages { get; set; }

            public Action<string> OnTimeToProcess { get; set; }
            public Action<string> OnBucketEmpty { get; set; }

            public BaggageBucket(string flightNumber, int time, ITimerService timerService) : base(timerService)
            {
                this._flightNumber = flightNumber;
                this._timeToFLight = time;
                this.Baggages = new Stack<Baggage>();
            }

            //TODO Implement timeToProcess()
            private void timeToProcess()
            {
                _timer = new Timer(DEFAULT_MOVING_TIME / TimerService.SimulationMultiplier);
                _timer.Elapsed += (sender,args) => passToRobot();
            }

            internal void passToRobot()
            {
                _timer.Stop();
                OnTimeToProcess(_flightNumber);
                _timer.Start();
            }

            private void add(Baggage baggage)
            {
                this.Baggages.Push(baggage);
            }

            public override void PassBaggage(Baggage baggage)
            {
                this.add(baggage);
            }
        }
        #endregion

        #region Robot

        internal enum RobotStatus
        {
            Inbound = 0,
            Outbound = 1
        }

        internal class RobotBSU : TransportingNode, ITransportingNode
        {
            private BSU _bsu;

            public RobotBSU(ITimerService timerService, BSU bsu) : base(1, timerService)
            {
                this._bsu = bsu;
            }

            public override void PassBaggage(Baggage baggage)
            {
                this.PassBaggage(baggage, RobotStatus.Outbound);
            }

            public void PassBaggage(Baggage baggage, RobotStatus status)
            {
                if(status == RobotStatus.Outbound)
                {
                    this.NextLink = _bsu._outboundConveyor;
                }
                else
                {
                    this.NextLink = _bsu._baggageBuckets[baggage.FlightNumber];
                }
                

                base.PassBaggage(baggage);
                
            }
        }
        #endregion
    }


}
