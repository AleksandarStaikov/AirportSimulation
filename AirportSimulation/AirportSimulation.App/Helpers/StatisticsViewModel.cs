namespace AirportSimulation.App.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using AirportSimulation.Core;
    using AirportSimulation.Core.Contracts;
    using AirportSimulation.Core.Services;

    partial class StatisticsViewModel : INotifyPropertyChanged
    {
        private static Func<StatisticsData> _calculateStatistics;

        public StatisticsData statisticsData = new StatisticsData();
        static DispatcherTimer timer = new DispatcherTimer();
        public int myVar;

        public StatisticsViewModel()
        {
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;

            collectedBags = new ObservableCollection<StatisticsModel>();

            dispatchedBags = new ObservableCollection<StatisticsModel>();

            pscFailSuccChart = new ObservableCollection<StatisticsModel>();

            ascFailSuccChart = new ObservableCollection<StatisticsModel>();

            overalPercSecurityChart = new ObservableCollection<StatisticsModel>();

            totalTransferBags = new ObservableCollection<StatisticsModel>();

            totalBsuChart = new ObservableCollection<StatisticsModel>();

            delaysAtAaChart = new ObservableCollection<StatisticsModel>();

            bsuStayTimeChart = new ObservableCollection<StatisticsModel>();

            noBsuChart = new ObservableCollection<StatisticsModel>();
            transportingTimeChart = new ObservableCollection<StatisticsModel>();

        }

        public static void StartStatisticsTimer()
        {
            _calculateStatistics = ContainerConfig.Resolve<IEngine>().GetStatisticsCalculator();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            statisticsData = _calculateStatistics();

            // PSC Failed or Succeed Chart
            pscFailSuccChart.Add(new StatisticsModel() { Category = "Failed", Number = statisticsData?.PscFailedBags?.Count ?? 0 });
            pscFailSuccChart.Add(new StatisticsModel() { Category = "Succeeded", Number = statisticsData?.PscSucceededBags?.Count ?? 0 });

            // ASC Failed or Succeed Chart
            ascFailSuccChart.Add(new StatisticsModel() { Category = "Failed", Number = statisticsData?.AscFailedBags?.Count ?? 0 });
            ascFailSuccChart.Add(new StatisticsModel() { Category = "Succeeded", Number = statisticsData?.AscSucceededBags?.Count ?? 0 });

            // Overall Security Chart
            overalPercSecurityChart.Add(new StatisticsModel() { Category = "Advanced", Number = (float)statisticsData.AscInvalidationPercentage });
            overalPercSecurityChart.Add(new StatisticsModel() { Category = "Primary", Number = (float)statisticsData.PscInvalidationPercentage });

            // Total Bags at BSU
            totalBsuChart.Add(new StatisticsModel() { Category = "Total Bags That Went To BSU", Number = statisticsData?.TotalBagsThatWentToBsu?.Count ?? 0 });

            // Total Transfer Bags
            totalTransferBags.Add(new StatisticsModel() { Category = "Total Transferred Bags", Number = statisticsData?.TotalTransferredBags?.Count ?? 0 });

            // Total Delays at Airport Security
            delaysAtAaChart.Add(new StatisticsModel() { Category = "Delays per Flight", Number = statisticsData?.TotalBagsArrivedLateAtAa?.Count ?? 0 });

            //Collected Bags
            //collectedBags.Add(new StatisticsModel() { Category = "First", Number = Convert.ToInt32(statisticsData?.FirstCollectedBag?.Log?.FirstOrDefault()) });
            collectedBags.Add(new StatisticsModel() { Category = "First", Number = Convert.ToInt32(statisticsData?.FirstCollectedBag) });
            collectedBags.Add(new StatisticsModel() { Category = "Last", Number = Convert.ToInt32(statisticsData?.LastCollectedBag)});

            //Dispatched Bags
            dispatchedBags.Add(new StatisticsModel() { Category = "First", Number = Convert.ToInt32(statisticsData?.FirstDispatchedBag?.Log?.FirstOrDefault()?.LogCreated.TotalSeconds)});
            dispatchedBags.Add(new StatisticsModel() { Category = "Last", Number = Convert.ToInt32(statisticsData?.LastDispatchedBag?.Log?.LastOrDefault()?.LogCreated.TotalSeconds) });


            //BSU Stay Time
           
                bsuStayTimeChart.Add(new StatisticsModel() { Category = "Average", Number = (float)statisticsData?.AverageBsuStayTimeInSeconds });
                bsuStayTimeChart.Add(new StatisticsModel() { Category = "Max", Number = (float)statisticsData?.MaxBsuStayTimeInSeconds });
                bsuStayTimeChart.Add(new StatisticsModel() { Category = "Min", Number = (float)statisticsData?.MinBsuStayTimeInSeconds });
            

            //System Stay Without BSU.
            noBsuChart.Add(new StatisticsModel() { Category = "Longest", Number = (float)statisticsData?.LongestSystemStayWithoutBsu });
            noBsuChart.Add(new StatisticsModel() { Category = "Shortest", Number = (float)statisticsData?.ShortestSystemStayWithoutBsu });

            transportingTimeChart.Add(new StatisticsModel() { Category = "Shortest", Number = (float)statisticsData?.ShortestTransportingTime });
            transportingTimeChart.Add(new StatisticsModel() { Category = "Longest", Number = (float)statisticsData?.LongestTransportingTime });


            //foreach (var flight in Series)
            //{
            //    totalBsuChart.Add(new StatisticsModel() { Category = flight.Key.FlightNumber, Number = (float)flight.Value });
            //}



        }

        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        public List<SeriesData> Series
        {
            get;
            set;
        }
        // for column charts
        public ObservableCollection<StatisticsModel> dispatchedBags
        {
            get;
            set;
        }
        // BSU Graphs

        // BSU Graphs
        public ObservableCollection<StatisticsModel> noBsuChart { get; set; }

        public ObservableCollection<StatisticsModel> transportingTimeChart { get; set; }

        public ObservableCollection<StatisticsModel> pscFailSuccChart { get; set; }

        public ObservableCollection<StatisticsModel> ascFailSuccChart { get; set; }

        public ObservableCollection<StatisticsModel> overalPercSecurityChart { get; set; }

        public ObservableCollection<StatisticsModel> totalTransferBags { get; set; }

        public ObservableCollection<StatisticsModel> totalBsuChart { get; set; }

        public ObservableCollection<StatisticsModel> bsuStayTimeChart { get; set;}

        public ObservableCollection<StatisticsModel> delaysAtAaChart { get; set; }

        public ObservableCollection<StatisticsModel> collectedBags { get; set; }

        public ObservableCollection<StatisticsModel> lastBags { get; set; }


        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
