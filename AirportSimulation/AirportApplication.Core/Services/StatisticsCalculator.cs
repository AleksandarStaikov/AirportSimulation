namespace AirportSimulation.Core.Services
{
    using System;
    using Common.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class StatisticsCalculator
    {
        public StatisticsData CalculateStatistics()
        {
            var baggage = Baggage.AllBaggage;

            var statisticsData = new StatisticsData();

            SetDispatchedTimes(statisticsData, baggage);
            SetCollectedTimes(statisticsData, baggage);
            SetPscSucceededAndFailed(statisticsData, baggage);
            SetAscSucceededAndFailed(statisticsData, baggage);

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

        private void SetPscSucceededAndFailed(StatisticsData data, List<Baggage> baggages)
        {
            data.PscFailedBags = baggages.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.PscCheckFailed))).ToList();
            data.PscSucceededBags = baggages.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.PscCheckSucceeded))).ToList();
        }

        private void SetAscSucceededAndFailed(StatisticsData data, List<Baggage> baggages)
        {
            data.AscFailedBags = baggages.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.AscCheckFailed))).ToList();
            data.AscSucceededBags = baggages.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.AscCheckSucceeded))).ToList();
        }

        private void SetFlightDelays(StatisticsData data, List<Baggage> baggages)
        {
            data.TotalBagsArrivedLateAtAA = baggages.Where(b =>
                b.Log.Any(log => log.Description.Contains(LoggingConstants.BagArrivedLateAtAirportArea))).ToList();

            data.BaggsLateForFlightPerFlight = data.TotalBagsArrivedLateAtAA.GroupBy(b => b.Flight);

            //Its fucked up, I know but that's the query, if you can make it more readable or faster, go ahead
            data.DelaysPerFlight = data
                .BaggsLateForFlightPerFlight
                .ToDictionary(x => x.Key, y =>
                    y.Max(bag =>
                        double.Parse(bag.Log
                            .FirstOrDefault(x => x.Description.Contains(LoggingConstants.BagArrivedLateAtAirportArea))
                            .Description.Split(new string[] { " with ", " minutes" }, StringSplitOptions.RemoveEmptyEntries)[1])));
        }
    }

    public class StatisticsData
    {
        public Baggage FirstDispatchedBag { get; set; }
        public Baggage LastDispatchedBag { get; set; }

        public Baggage FirstCollectedBag { get; set; }
        public Baggage LastCollectedBag { get; set; }

        public List<Baggage> PscFailedBags { get; set; }
        public List<Baggage> PscSucceededBags { get; set; }

        public List<Baggage> AscFailedBags { get; set; }
        public List<Baggage> AscSucceededBags { get; set; }

        public double PscInvalidationPercentage =>
            (100D / (double)(PscSucceededBags.Count + PscFailedBags.Count)) * PscFailedBags.Count;
        public double AscInvalidationPercentage =>
            (100D / (double)(AscSucceededBags.Count + AscFailedBags.Count)) * AscFailedBags.Count;

        public List<Baggage> TotalBagsArrivedLateAtAA { get; set; }

        public IEnumerable<IGrouping<Flight, Baggage>> BaggsLateForFlightPerFlight { get; set; }

        public Dictionary<Flight, double> DelaysPerFlight { get; set; }
    }
}
