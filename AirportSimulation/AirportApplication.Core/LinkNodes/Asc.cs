namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Asc : ProcessingNode, IProcessingNode
    {
        public delegate Asc Factory();

        public Asc(ITimerService timerService) : base(timerService)
        {
        }

        public ChainLink FailSuccessor { get; set; }

        public override void Process(Baggage baggage)
        {
            //TODO : Implment
            var checkSuccessful = new Random().Next(0,1) > 0;

            if (checkSuccessful)
            {
                NextLink.PassBaggage(baggage);
            }
            else
            {
                FailSuccessor.PassBaggage(baggage);
            }
        }

    }
}
