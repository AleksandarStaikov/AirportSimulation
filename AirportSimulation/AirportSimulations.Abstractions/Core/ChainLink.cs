namespace AirportSimulation.Abstractions.Core
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ChainLink : IChainLink
    {
        protected List<Action> _subscribers;

        public ChainLink()
        {
            _subscribers = new List<Action>();
        }

        public ChainLink SuccessSuccessor { get; set; }

        public ChainLink Predecessor { get; set; }

        public abstract bool CanProcess();
        public abstract void Process();

        public void SubscribeOnDone(Action methodToCall)
        {
            _subscribers.Add(methodToCall);
        }

        public void UnSubscribeOnDone(Action methodToUnsubsctibe)
        {
            _subscribers.Remove(methodToUnsubsctibe);
        }

        public void OnDone()
        {
            if (_subscribers.Any())
            {
                _subscribers.First().Invoke();
                _subscribers.RemoveAt(0);
            }
        }
    }
}
