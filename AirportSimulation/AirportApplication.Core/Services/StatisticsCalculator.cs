namespace AirportSimulation.Core.Services
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using LinkNodes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Threading;

    public class StatisticsCalculator// : IStatisticsCalculator
    {
        public static StatisticsData CalculateStatistics(ISimulationSettings simulationSettings)
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

            SetTransportingTimeRelatedData(statisticsData, baggage);

            return statisticsData;
        }

        private static void SetDispatchedTimes(StatisticsData data, List<Baggage> baggage)
        {
            var orderedByFirstLogTime = baggage.OrderBy(b => b.Log[0]?.LogCreated)?.ToList();

            data.FirstDispatchedBag = orderedByFirstLogTime.FirstOrDefault();
            data.LastDispatchedBag = orderedByFirstLogTime.LastOrDefault();
        }

        private static void SetCollectedTimes(StatisticsData data, List<Baggage> baggage)
        {
            var orderedByFirstLogTime = baggage.OrderBy(b => b.Log.LastOrDefault()?.LogCreated)?.ToList();

            data.FirstCollectedBag = orderedByFirstLogTime.FirstOrDefault().Log.FirstOrDefault().LogCreated.TotalSeconds;
            data.LastCollectedBag = orderedByFirstLogTime.FirstOrDefault().Log.LastOrDefault().LogCreated.TotalSeconds;
        }

        private static void SetPscSucceededAndFailed(StatisticsData data, List<Baggage> baggage)
        {
            data.PscFailedBags = baggage.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.PscCheckFailed))).ToList();
            data.PscSucceededBags = baggage.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.PscCheckSucceeded))).ToList();
        }

        private static void SetAscSucceededAndFailed(StatisticsData data, List<Baggage> baggage)
        {
            data.AscFailedBags = baggage.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.AscCheckFailed))).ToList();
            data.AscSucceededBags = baggage.Where(b => b.Log.Any(log => log.Description.Contains(LoggingConstants.AscCheckSucceeded))).ToList();
        }

        private static void SetFlightDelays(StatisticsData data, List<Baggage> baggage)
        {
            data.TotalBagsArrivedLateAtAa = baggage.Where(b =>
                b.Log.Any(log => log.LateForFlight.HasValue)).ToList();

            data.BagsLateForFlightPerFlight = data.TotalBagsArrivedLateAtAa.GroupBy(b => b.Flight);

            data.DelaysPerFlight = data
                .BagsLateForFlightPerFlight
                .ToDictionary(x => x.Key, y => y.Max(bag => bag.Log.FirstOrDefault(log => log.LateForFlight.HasValue).LateForFlight.Value));
        }

        private static void SetTransferredBagsCount(StatisticsData data, List<Baggage> baggages)
        {
            data.TotalTransferredBags = baggages.Where(bag =>
                bag.Log.Any(log => log.Description.Contains(LoggingConstants.BagRedirectedToAnotherFlight))).ToList();
        }

        private static void SetBsuRelatedStatistics(StatisticsData data, List<Baggage> baggages)
        {
            data.TotalBagsThatWentToBsu =
                baggages.Where(bag => bag.Log.Any(log => log.Description.Contains(typeof(BSU).Name))).ToList();



            data.AverageBsuStayTimeInSeconds = data.TotalBagsThatWentToBsu.DefaultIfEmpty().Average(b => GetBsuStayTimeInSeconds(b));
            data.MinBsuStayTimeInSeconds = data.TotalBagsThatWentToBsu.DefaultIfEmpty().Min(b => GetBsuStayTimeInSeconds(b));
            data.MaxBsuStayTimeInSeconds = data.TotalBagsThatWentToBsu.DefaultIfEmpty().Max(b => GetBsuStayTimeInSeconds(b));



            data.AverageBsuStayTimeInMinutes = data.TotalBagsThatWentToBsu.DefaultIfEmpty().Average(b => GetBsuStayTime(b));

            data.MinBsuStayTimeInMinutes = (double)data.TotalBagsThatWentToBsu.DefaultIfEmpty().Min(b => GetBsuStayTime(b));

            data.MaxBsuStayTimeInMinutes = (double)data.TotalBagsThatWentToBsu.DefaultIfEmpty().Max(b => GetBsuStayTime(b));

            data.LongestSystemStayWithoutBsu = baggages.DefaultIfEmpty().Max(bag =>
                (bag.Log.LastOrDefault().LogCreated - bag.Log.FirstOrDefault().LogCreated).TotalSeconds - GetBsuStayTimeInSeconds(bag));
            
            data.ShortestSystemStayWithoutBsu = baggages.DefaultIfEmpty().Min(bag =>
                (bag.Log.LastOrDefault().LogCreated - bag.Log.FirstOrDefault().LogCreated).TotalSeconds - GetBsuStayTimeInSeconds(bag));
        }

        private static void SetTransportingTimeRelatedData(StatisticsData data, List<Baggage> baggage)
        {
            var allTransportingLogs = baggage.SelectMany(b => b.Log)
                .Where(log => log.Description.Contains(LoggingConstants.BagReceivedText)).ToList();

            data.LongestTransportingTime = allTransportingLogs.DefaultIfEmpty().Max(log => log.TimeElapsed.TotalSeconds);

            data.ShortestTransportingTime = allTransportingLogs.DefaultIfEmpty().Min(log => log?.TimeElapsed.TotalSeconds ?? 0);

            var nodes = ChainLinkFactory.Nodes.OfType<ITransportingNode>().ToList();
            var transportationWaits = allTransportingLogs.Select(log => CalculateLogWait(log, nodes)).OrderBy(t => t.Item2).ToList();

            data.MinWaitingTimeAtTransporterOrQueue = transportationWaits.FirstOrDefault();
            data.MaxWaitingTimeAtTransporterOrQueue = transportationWaits.LastOrDefault();

            data.AverageWaitingTimePerTransporterOrQueue = transportationWaits
                .GroupBy(pair => pair.Item1, pair => pair.Item2)
                .Select(g => new KeyValuePair<string, double>(g.Key, g.Average(v => v)))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private static Tuple<string, double> CalculateLogWait(BaggageEventLog log, IEnumerable<ITransportingNode> nodes)
        {
            var transporterIdentifier = log.Description.Split(new[] { LoggingConstants.BagTransporterIdText }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()[1];
            if (log.Description.Contains("Queue"))
            {
                return new Tuple<string, double>(transporterIdentifier, log.TimeElapsed.TotalSeconds);
            }
            else
            {
                var transportingNode = nodes.FirstOrDefault(n => n.NodeId == transporterIdentifier);
                var transporterNominalTimeInMilliseconds = transportingNode?.Length ?? 0 * transportingNode?.MoveTime ?? 0;
                var actualTransportingTime = log.TimeElapsed.Milliseconds;
                var transportationOverload = actualTransportingTime - transporterNominalTimeInMilliseconds;
                return new Tuple<string, double>(transporterIdentifier,
                    TimeSpan.FromMilliseconds(transportationOverload).TotalSeconds);
            }
        }

        private static double GetBsuStayTime(Baggage bag)
        {
            if (bag == null)
            {
                return 0;
            }

            var receivedInBsuText = string.Format(LoggingConstants.BagReceivedInTemplate, typeof(BSU).Name, bag.TransporterId);
            var receivedInRobotFromBucket = string.Format(LoggingConstants.ReceivedInRobotSendingTo, typeof(Mpa).Name, bag.TransporterId);

            var startTime = bag.Log.FirstOrDefault(log => log.Description.Contains(receivedInBsuText))?.LogCreated ?? TimeSpan.Zero;
            var endTime = bag.Log.FirstOrDefault(log => log.Description.Contains(receivedInRobotFromBucket))?.LogCreated ?? TimeSpan.Zero;

            return (endTime - startTime).TotalMinutes;
        }

        private static double GetBsuStayTimeInSeconds(Baggage bag)
        {
            if (bag == null)
            {
                return 0;
            }

            var receivedInBsuText = string.Format(LoggingConstants.BagReceivedInTemplate, typeof(BSU).Name, bag.TransporterId);
            var receivedInRobotFromBucket = string.Format(LoggingConstants.ReceivedInRobotSendingTo, typeof(Mpa).Name, bag.TransporterId);

            var startTime = bag.Log.FirstOrDefault(log => log.Description.Contains(receivedInBsuText))?.LogCreated ?? TimeSpan.Zero;
            var endTime = bag.Log.FirstOrDefault(log => log.Description.Contains(receivedInRobotFromBucket))?.LogCreated ?? TimeSpan.Zero;

            return endTime.TotalSeconds - startTime.TotalSeconds;
        }

     
       

    }

    public class StatisticsData
    {

        //Colum Chart -- DONE
        public Baggage FirstDispatchedBag { get; set; }
        public Baggage LastDispatchedBag { get; set; }
        public double FirstCollectedBag { get; set; }
        public double LastCollectedBag { get; set; }

        //Pie chart
        public List<Baggage> PscFailedBags { get; set; }
        public List<Baggage> PscSucceededBags { get; set; }

        //Pie chart
        public List<Baggage> AscFailedBags { get; set; }
        public List<Baggage> AscSucceededBags { get; set; }

        //Pie Chart
        public double PscInvalidationPercentage =>
            (100D / (double)(PscSucceededBags.Count + PscFailedBags.Count)) * PscFailedBags.Count;
        public double AscInvalidationPercentage =>
            (100D / (double)(AscSucceededBags.Count + AscFailedBags.Count)) * AscFailedBags.Count;

        //Column Chart
        public List<Baggage> TotalBagsArrivedLateAtAa { get; set; }

        public IEnumerable<IGrouping<IFlight, Baggage>> BagsLateForFlightPerFlight { get; set; }

        public Dictionary<IFlight, double> DelaysPerFlight { get; set; }

        public List<Baggage> TotalTransferredBags { get; set; }

        public List<Baggage> TotalBagsThatWentToBsu { get; set; }

        public double AverageBsuStayTimeInSeconds { get; set; }
        public double MinBsuStayTimeInSeconds { get; set; }
        public double MaxBsuStayTimeInSeconds { get; set; }

        //Column
        public double AverageBsuStayTimeInMinutes { get; set; }
        public double MinBsuStayTimeInMinutes { get; set; }
        public double MaxBsuStayTimeInMinutes { get; set; }

        //Column
        public double LongestSystemStayWithoutBsu { get; set; }
        public double ShortestSystemStayWithoutBsu { get; set; }
        public double LongestTransportingTime { get; set; }
        public double ShortestTransportingTime { get; set; }

        public Tuple<string, double> MinWaitingTimeAtTransporterOrQueue { get; set; }
        public Tuple<string, double> MaxWaitingTimeAtTransporterOrQueue { get; set; }
        public Dictionary<string, double> AverageWaitingTimePerTransporterOrQueue { get; set; }
    }
}
