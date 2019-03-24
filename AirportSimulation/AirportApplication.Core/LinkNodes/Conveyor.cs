namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;

    class Conveyor : TransportingNode, ITransportingNode
    {
        public Conveyor(int length, ITimerService timerService)
            : base(length, timerService)
        {
        }
    }
}
