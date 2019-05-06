namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using System.Collections.Generic;
    using Common.Models;
    using System;
    using System.Threading.Tasks;

    public class Mpa : ChainLink, IChainLink
    {
        public delegate Mpa Factory();

        private Dictionary<string, ChainLink> _allSuccessors;
        private Dictionary<string, Queue<Baggage>> _baggageDistributors;

        public Mpa(ITimerService timerService) : base(timerService)
        {
            _allSuccessors = new Dictionary<string, ChainLink>();
            _baggageDistributors = new Dictionary<string, Queue<Baggage>>();

        }

        public override string Destination => this.GetType().Name;

        public void AddSuccessor(ChainLink successor)
        {
            _allSuccessors[successor.Destination] = successor;
            _baggageDistributors[successor.Destination] = new Queue<Baggage>();
        }

        public override void PassBaggage(Baggage baggage)
        {
            _baggageDistributors[baggage.Flight.Gate].Enqueue(baggage);
        }

        public void Start()
        {
            foreach(string destination in _baggageDistributors.Keys)
            {
                Task.Factory.StartNew(() =>
                {
                    distributeBaggage(destination, _baggageDistributors[destination]);
                });
            }
        }

        private void distributeBaggage(string destination, Queue<Baggage> baggages)
        {
            ChainLink nextNode = _allSuccessors[destination];
           
            while (true)
            {
                if (baggages.Count > 0)
                {
                    if (nextNode.Status == NodeState.Free)
                    {
                        nextNode.PassBaggage(baggages.Dequeue());
                    }
                    else
                    {
                        nextNode.OnStatusChangedToFree += () => { nextNode.PassBaggage(baggages.Dequeue()); };
                    }
                }
            }
        }
    }
}
