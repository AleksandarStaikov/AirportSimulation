namespace AirportSimulation.Core.Services
{
    using Common.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class StatisticsCalculator
    {
        public StatisticsData CalculateStatistics(List<Baggage> baggages)
        {
            var statisticsData = new StatisticsData();

            SetDispatchedTimes(statisticsData, baggages);
            SetCollectedTimes(statisticsData, baggages);

            return statisticsData;
        }

        private void SetDispatchedTimes(StatisticsData data, List<Baggage> baggages)
        {
            var orderedByFirstLogTime = baggages.OrderBy(b => b.Log[0].LogCreated);

            data.FirstDispatchedBag = orderedByFirstLogTime.First();
            data.LastDispatchedBag = orderedByFirstLogTime.Last();
        }

        private void SetCollectedTimes(StatisticsData data, List<Baggage> baggages)
        {
            var orderedByFirstLogTime = baggages.OrderBy(b => b.Log.Last().LogCreated);

            data.FirstCollectedBag = orderedByFirstLogTime.First();
            data.LastCollectedBag = orderedByFirstLogTime.Last();
        }
    }

    public class StatisticsData
    {
        public Baggage FirstDispatchedBag { get; set; }
        public Baggage LastDispatchedBag { get; set; }

        public Baggage FirstCollectedBag { get; set; }
        public Baggage LastCollectedBag { get; set; }
    }
}
