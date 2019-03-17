namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Core;
    using Abstractions.Core.Contracts;

    class Conveyor : TransportingNode, ITransportingNode
    {
        public Conveyor(int length) : base(length)
        {
        }
    }
}
