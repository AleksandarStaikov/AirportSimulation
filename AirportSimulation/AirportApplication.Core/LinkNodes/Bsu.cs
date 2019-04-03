namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System.Collections.Generic;

    public class BSU : ChainLink, IChainLink
    {
        internal Dictionary<string, BaggageBucket> _baggageBuckets;
        internal Conveyor _inboundConveyor;
        internal Conveyor _outboundConveyor;
        internal RobotBSU _robot;

        public BSU(ITimerService timerService) : base(timerService)
        {
            _baggageBuckets = new Dictionary<string, BaggageBucket>();
            _inboundConveyor = new Conveyor(5, timerService); //TODO variable conveyor capacity
            _outboundConveyor = new Conveyor(5, timerService); //TODO variable conveyor capacity
            _robot = new RobotBSU(1, timerService, this); //Lenght is always 1

            _inboundConveyor.Predecessor = this.Predecessor;
            _inboundConveyor.SuccessSuccessor = _robot;
            _outboundConveyor.Predecessor = _robot;
            _outboundConveyor.SuccessSuccessor = this.SuccessSuccessor;
        }

        public void AddBaggage(Baggage baggage)
        {
            if(!_baggageBuckets.ContainsKey(baggage.FlightNumber))
            {
                _baggageBuckets.Add(baggage.FlightNumber, new BaggageBucket(baggage.FlightNumber, 1, this.TtimerService)); //TODO Variable timeToFlight
                _baggageBuckets[baggage.FlightNumber].timeToProcessHandler += new BaggageBucket.onTimeToProcess(_outboundConveyor.PassBaggage); //TODO Implement
            }
            else
            {
                _baggageBuckets[baggage.FlightNumber].Add(baggage);
            }
            
        }

        public override void PassBaggage(Baggage baggage)
        {
            _inboundConveyor.PassBaggage(baggage);
        }

        #region BaggageBucket

        //Possibly externalize this class. No need so far, as only used in BSU
        internal class BaggageBucket : ChainLink, IChainLink
        {
            private string _flightNumber;
            private int _timeToFLight;
            public List<Baggage> Baggages { get; set; }

            public delegate void onTimeToProcess(Baggage baggage);
            public event onTimeToProcess timeToProcessHandler;


            public BaggageBucket(string flightNumber, int time, ITimerService timerService) : base(timerService)
            {
                this._flightNumber = flightNumber;
                this._timeToFLight = time;
                this.Baggages = new List<Baggage>();
            }

            //TODO Implement timeToProcess()

            public void Add(Baggage baggage)
            {
                this.Baggages.Add(baggage);
            }

            public override void PassBaggage(Baggage baggage)
            {
                this.Add(baggage);
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
            RobotStatus status = RobotStatus.Inbound;

            public RobotBSU(int length, ITimerService timerService, BSU bsu) : base(length, timerService)
            {
                this._bsu = bsu;
            }

            public override void PassBaggage(Baggage baggage)
            {
                this.PassBaggage(baggage, RobotStatus.Inbound);
            }

            public void PassBaggage(Baggage baggage, RobotStatus status)
            {
                if(status == RobotStatus.Outbound)
                {
                    this.Predecessor = _bsu._baggageBuckets[baggage.FlightNumber];
                    this.SuccessSuccessor = _bsu._outboundConveyor;
                }
                else
                {
                    this.Predecessor = _bsu._inboundConveyor;
                    this.SuccessSuccessor = _bsu._outboundConveyor;
                }
                

                base.PassBaggage(baggage);
                
            }
        }
        #endregion
    }


}
