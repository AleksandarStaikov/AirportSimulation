namespace AirportSimulation.Abstractions.Core.Contracts
{
    public interface ITransportingNode: IChainLink
    {
        IChainLink NextNode { get; set; }

        void Start();

        void Stop();
    }
}