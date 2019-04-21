namespace AirportSimulation.Core.Contracts
{
    using Common.Models;

    public interface IEngine
    {
        void Run(SimulationSettings settings);
    }
}