namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;

    public class Conveyor : TransportingNode, ITransportingNode
    {
        public delegate Conveyor Factory(int length);

        public Conveyor(int length, ITimerService timerService)
            : base(length, timerService)
        {
        }
    }
}
