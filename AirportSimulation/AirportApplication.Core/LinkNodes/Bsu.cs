namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using System.Collections.Generic;

    public class Bsu : ProcessingNode, IProcessingNode
    {
        private Dictionary<string, BaggageBucket> _baggageBuckets;
        private Conveyor _inboundConveyor;
        private Conveyor _outboundConveyor;
        private RobotBSU _robot;

        public Bsu(ITimerService timerService) : base(timerService)
        {
            _baggageBuckets = new Dictionary<string, BaggageBucket>();
            _inboundConveyor = new Conveyor(5, timerService); //TODO variable conveyor capacity
            _outboundConveyor = new Conveyor(5, timerService); //TODO variable conveyor capacity
            _robot = new RobotBSU(1, timerService); //Lenght is always 1

            _inboundConveyor.Predecessor = this.Predecessor;
            _inboundConveyor.SuccessSuccessor = _robot;
            _outboundConveyor.Predecessor = _robot;
            _outboundConveyor.SuccessSuccessor = this.SuccessSuccessor;
        }

        public void AddBaggage(string flightNumber, int timeToFlight) //TODO Implement with Baggage
        {
            //TODO Check for existing
            _baggageBuckets.Add(flightNumber, new BaggageBucket(flightNumber, timeToFlight, this.TtimerService));
            _baggageBuckets[flightNumber].timeToProcessHandler += new BaggageBucket.onTimeToProcess(moveToOutbound);
        }

        private void moveToOutbound(string flightNumber)
        {
           //TODO implement
        }

        public override void PassBaggage(Baggage baggage)
        {
            _inboundConveyor.PassBaggage(baggage);
        }

        public override void ProcessInternal(Baggage baggage)
        {
            throw new System.NotImplementedException();
        }

        #region BaggageBucket
        //Possibly externalize this class. No need so far, as only used in BSU
        //Possibly change inheritance to ProcessingNode
        internal class BaggageBucket : ChainLink, IChainLink
        {
            private string _flightNumber;
            private int _timeToFLight;
            public List<Baggage> Baggages { get; set; }

            public delegate void onTimeToProcess(string flightNumber);
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

        internal class RobotBSU : TransportingNode, ITransportingNode
        {
            public RobotBSU(int length, ITimerService timerService) : base(length, timerService) { }

            public void PassBaggage(Baggage baggage, ChainLink predecessor, ChainLink successor)
            {
                this.Predecessor = predecessor;
                this.SuccessSuccessor = successor;

                base.PassBaggage(baggage);
                
            }

            
        }
    }

   
}
