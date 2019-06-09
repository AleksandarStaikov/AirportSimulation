namespace AirportSimulation.Core.Contracts
{
    using AirportSimulation.Core.Services;
    using Common.Models;
    using System;

    public interface IEngine
    {
        void Run(SimulationSettings settings);
        void RunDemo(SimulationSettings settings);
        Func<StatisticsData> GetStatisticsCalculator();
    }
}