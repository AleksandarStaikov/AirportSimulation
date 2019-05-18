namespace AirportSimulation.Core.Services
{
    using System;
    using Common.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Common.Models.Contracts;
    using LinkNodes;

    public class StatisticsCalculator
    {
        public StatisticsData CalculateStatistics(ISimulationSettings simulationSettings)
        {
            var baggage = Baggage.AllBaggage;

            var statisticsData = new StatisticsData();

            SetDispatchedTimes(statisticsData, baggage);
            SetCollectedTimes(statisticsData, baggage);
            SetPscSucceededAndFailed(statisticsData, baggage);
            SetAscSucceededAndFailed(statisticsData, baggage);
            SetFlightDelays(statisticsData, baggage);
            SetTransferredBagsCount(statisticsData, baggage);
            SetBsuRelatedStatistics(statisticsData, baggage);

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
            data.TotalBagsArrivedLateAtAa = baggages.Where(b =>
                b.Log.Any(log => log.LateForFlight.HasValue)).ToList();

            data.BagsLateForFlightPerFlight = data.TotalBagsArrivedLateAtAa.GroupBy(b => b.Flight);

            data.DelaysPerFlight = data
                .BagsLateForFlightPerFlight
                .ToDictionary(x => x.Key, y => y.Max(bag => bag.Log.FirstOrDefault(log => log.LateForFlight.HasValue).LateForFlight.Value));
        }

        private void SetTransferredBagsCount(StatisticsData data, List<Baggage> baggages)
        {
            data.TotalTransferredBags = baggages.Where(bag =>
                bag.Log.Any(log => log.Description.Contains(LoggingConstants.BagRedirectedToAnotherFlight))).ToList();
        }

        private void SetBsuRelatedStatistics(StatisticsData data, List<Baggage> baggages)
        {
            data.TotalBagsThatWentToBsu =
                baggages.Where(bag => bag.Log.Any(log => log.Description.Contains(typeof(BSU).Name))).ToList();

            data.AverageBsuStayTimeInMinutes = data.TotalBagsThatWentToBsu.Average(GetBsuStayTime);

            data.MinBsuStayTimeInMinutes = data.TotalBagsThatWentToBsu.Min(GetBsuStayTime);

            data.MaxBsuStayTimeInMinutes = data.TotalBagsThatWentToBsu.Max(GetBsuStayTime);

            data.LongestSystemStayWithoutBsu = baggages.Max(bag =>
                (bag.Log.Last().LogCreated - bag.Log.Last().LogCreated).TotalMinutes - GetBsuStayTime(bag));

            data.ShortestSystemStayWithoutBsu = baggages.Min(bag =>
                (bag.Log.Last().LogCreated - bag.Log.Last().LogCreated).TotalMinutes - GetBsuStayTime(bag));
        }

        private void SetTransportingTimeRelatedData(StatisticsData data, List<Baggage> baggages)
        {
            data.LongestTransportingTime = baggages.Max(bag =>
                bag.Log.Where(log => log.Description.Contains(LoggingConstants.BagReceivedText))
                       .Max(log => log.TimeElapsed.TotalMinutes));

            data.ShortestTransportingTime = baggages.Min(bag =>
                bag.Log.Where(log => log.Description.Contains(LoggingConstants.BagReceivedText))
                    .Min(log => log.TimeElapsed.TotalMinutes));
        }

        private double GetBsuStayTime(Baggage bag)
        {
            var receivedInBsuText = string.Format(LoggingConstants.BagReceivedInTemplate, typeof(BSU).Name, bag.TransporterId);
            var receivedInRobotFromBucket = string.Format(LoggingConstants.ReceivedInRobotSendingTo, typeof(Mpa).Name, bag.TransporterId);

            var startTime = bag.Log.FirstOrDefault(log => log.Description.Contains(receivedInBsuText))?.LogCreated ?? TimeSpan.Zero;
            var endTime = bag.Log.FirstOrDefault(log => log.Description.Contains(receivedInRobotFromBucket))?.LogCreated ?? TimeSpan.Zero;

            return (endTime - startTime).TotalMinutes;
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

        public List<Baggage> TotalBagsArrivedLateAtAa { get; set; }

        public IEnumerable<IGrouping<Flight, Baggage>> BagsLateForFlightPerFlight { get; set; }

        public Dictionary<Flight, double> DelaysPerFlight { get; set; }

        public List<Baggage> TotalTransferredBags { get; set; }

        public List<Baggage> TotalBagsThatWentToBsu { get; set; }

        public double AverageBsuStayTimeInMinutes { get; set; }
        public double MinBsuStayTimeInMinutes { get; set; }
        public double MaxBsuStayTimeInMinutes { get; set; }

        public double LongestSystemStayWithoutBsu { get; set; }
        public double ShortestSystemStayWithoutBsu { get; set; }

        public double LongestTransportingTime { get; set; }
        public double ShortestTransportingTime { get; set; }
    }
}
