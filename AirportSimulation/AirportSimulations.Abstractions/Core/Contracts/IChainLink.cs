namespace AirportSimulation.Abstractions.Core.Contracts
{
    using System;

    public interface IChainLink
    {
        ChainLink SuccessSuccessor { get; set; }
        ChainLink Predecessor { get; set; }
        bool CanProcess();
        void Process();
        void SubscribeOnDone(Action methodToCall);
        void UnSubscribeOnDone(Action methodToUnsubsctibe);
        void OnDone();
    }
}