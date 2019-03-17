namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;

    public class CheckInDesk : ProcessingNode, IProcessingNode
    {
        public override void ProcessInternal(Baggage baggage)
        {
            //TODO : Implment
            SuccessSuccessor.PassBaggage(_currentBaggage);
        }
    }
}
