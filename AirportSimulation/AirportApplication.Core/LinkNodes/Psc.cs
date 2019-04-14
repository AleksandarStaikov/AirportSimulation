namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Psc : ProcessingNode, IProcessingNode
    {
        public Psc(ITimerService timerService) : base(timerService)
        {
        }

        public ChainLink FailSuccessor { get; set; }

        public override void ProcessInternal(Baggage baggage)
        {
            //TODO : Implment
            bool CheckSuccessfull = new Random().Next(0, 1) > 0 ? true : false;

            if (CheckSuccessfull)
            {
                SuccessSuccessor.PassBaggage(baggage);
            }
            else
            {
                FailSuccessor.PassBaggage(baggage);
            }
        }

    }
}
