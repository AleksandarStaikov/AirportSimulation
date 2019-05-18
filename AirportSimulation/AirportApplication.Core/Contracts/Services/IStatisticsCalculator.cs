namespace AirportSimulation.Core.Contracts.Services
{
    using Common.Models.Contracts;
    using Core.Services;

    public interface IStatisticsCalculator
    {
        StatisticsData CalculateStatistics(ISimulationSettings simulationSettings);
    }
}