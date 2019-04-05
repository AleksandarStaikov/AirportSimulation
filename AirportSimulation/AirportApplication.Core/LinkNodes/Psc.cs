namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Psc : ProcessingNode, IProcessingNode
    {
        public delegate Psc Factory();

        public Psc(ITimerService timerService) : base(timerService)
        {
        }

        public ChainLink FailSuccessor { get; set; }

        public override void Process(Baggage baggage)
        {
            //TODO : Implment 
            //TODO : Check FailSuccessor state somewhere bruh
            bool CheckSuccessfull = new Random().Next(0, 2) > 0 ? true : false;

            if (true)
            {
                NextLink.PassBaggage(baggage);
            }
            else
            {
                //FailSuccessor.PassBaggage(baggage);
            }
        }

    }
}
