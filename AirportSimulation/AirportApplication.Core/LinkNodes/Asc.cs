namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class Asc : ProcessingNode, IProcessingNode
    {
        public ChainLink FailSuccessor { get; set; }

        public override void ProcessInternal(Baggage baggage)
        {
            //TODO : Implment
            var checkSuccessful = new Random().Next(0,1) > 0;

            if (checkSuccessful)
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
