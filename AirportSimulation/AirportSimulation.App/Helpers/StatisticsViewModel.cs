namespace AirportSimulation.App.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using AirportSimulation.Core.Services;


    class StatisticsViewModel : INotifyPropertyChanged
    {
        public StatisticsData statisticsData = new StatisticsData();

        public StatisticsViewModel()
        {

            Series = new List<SeriesData>();


            // gr = Graph --- Col = 1 --- 
            _gr1Col1 = new ObservableCollection<StatisticsModel>();
            _gr1Col2 = new ObservableCollection<StatisticsModel>();

            _gr2Par1 = new ObservableCollection<StatisticsModel>();

            _gr3Par1 = new ObservableCollection<StatisticsModel>();

            _gr4Par1 = new ObservableCollection<StatisticsModel>();

            _gr2Col1 = new ObservableCollection<StatisticsModel>();
            _gr2Col2 = new ObservableCollection<StatisticsModel>();

            _gr3Col1 = new ObservableCollection<StatisticsModel>();
            _gr3Col2 = new ObservableCollection<StatisticsModel>();

            _gr4Col1 = new ObservableCollection<StatisticsModel>();
            _gr4Col2 = new ObservableCollection<StatisticsModel>();


            //fir column chart

            _gr1Col1.Add(new StatisticsModel() { Category = "Collected Bags", Number = Convert.ToInt32(statisticsData?.FirstCollectedBag?.Log?.FirstOrDefault()) });
            _gr1Col1.Add(new StatisticsModel() { Category = "Collected Bags", Number = Convert.ToInt32(statisticsData?.LastCollectedBag?.Log?.FirstOrDefault()) });

            _gr1Col2.Add(new StatisticsModel() { Category = "Dispatched Bags", Number = Convert.ToInt32(statisticsData?.FirstDispatchedBag?.Log?.FirstOrDefault()) });
            _gr1Col2.Add(new StatisticsModel() { Category = "Dispatched Bags", Number = Convert.ToInt32(statisticsData?.LastDispatchedBag?.Log?.FirstOrDefault()) });

            // first pie chart
            _gr2Par1.Add(new StatisticsModel() { Category = "PSC Failed Percentage", Number = statisticsData?.PscFailedBags?.Count ?? 0 });
            _gr2Par1.Add(new StatisticsModel() { Category = "PSC Succeeded Percentage", Number = statisticsData?.PscFailedBags?.Count ?? 0 });
           
            //second pie chart
            _gr3Par1.Add(new StatisticsModel() { Category = "ASC Failed Percentage", Number = statisticsData?.AscFailedBags?.Count ?? 0 });
            _gr3Par1.Add(new StatisticsModel() { Category = "ASC Succeeded Percentage", Number = statisticsData?.AscFailedBags?.Count ?? 0 });

            //third pie chart - TO DO 
            // _gr4Par1.Add(new StatisticsModel() { Category = "PCS Invalidation Percentage", Number =}) -> can't convert

            //second column chart -> missing the flight per fligh variable commented out below
            _gr2Col1.Add(new StatisticsModel() { Category = "Total Bags Late at AA", Number = statisticsData?.TotalBagsArrivedLateAtAa?.Count ?? 0 });
            _gr2Col1.Add(new StatisticsModel() { Category = "Total Transferred Bags", Number = statisticsData?.TotalTransferredBags?.Count ?? 0 });

            _gr2Col2.Add(new StatisticsModel() { Category = "Total Bags At BSU", Number = statisticsData?.TotalBagsThatWentToBsu?.Count ?? 0 });
            _gr2Col2.Add(new StatisticsModel() { Category = "Delays per Flight", Number = statisticsData?.DelaysPerFlight?.Count ?? 0 });
            //_gr2Col2.Add(new StatisticsModel() { Category = "Late for flight per flight", Number = statisticsData?.BagsLateForFlightPerFlight?.Count ?? 0 }); - dont know how to convert


            // third column chart
            _gr3Col1.Add(new StatisticsModel() { Category = "Average BSU stay time ", Number = (float)statisticsData?.AverageBsuStayTimeInMinutes });
            _gr3Col1.Add(new StatisticsModel() { Category = "Max BSU stay time ", Number = (float)statisticsData?.MaxBsuStayTimeInMinutes });
            _gr3Col1.Add(new StatisticsModel() { Category = "Min BSU stay time ", Number = (float)statisticsData?.MinBsuStayTimeInMinutes });

            // fourth column chart
            _gr4Col1.Add(new StatisticsModel() { Category = "Longest Stay NO BSU ", Number = (float)statisticsData?.LongestSystemStayWithoutBsu});
            _gr4Col2.Add(new StatisticsModel() { Category = "Shortest Stay NO BSU ", Number = (float)statisticsData?.ShortestSystemStayWithoutBsu });
            _gr4Col1.Add(new StatisticsModel() { Category = "Longest Transporting Time", Number = (float)statisticsData?.LongestTransportingTime});
            _gr4Col2.Add(new StatisticsModel() { Category = "Shortest Transporting Time", Number = (float)statisticsData?.ShortestTransportingTime });
            
            //Series is the left column of the window ONLY  used for testing the numbers of different charts
            Series.Add(new SeriesData() { DisplayName = "First", Items = _gr1Col1 });
            Series.Add(new SeriesData() { DisplayName = "Last", Items = _gr1Col2 });
            
          //  Series.Add(new SeriesData() { DisplayName = "Pie Chart", Items = _gr2Par1 });
          //  Series.Add(new SeriesData() { DisplayName = "Pie Second", Items = _gr3Par1 });

            Series.Add(new SeriesData() { DisplayName = "Longest", Items = _gr4Col1 });
            Series.Add(new SeriesData() { DisplayName = "Shortest", Items = _gr4Col2 });
            //  Series.Add(new SeriesData() { DisplayName = "BSU bags", Items = _gr2Col2 });

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
        public ObservableCollection<StatisticsModel> _gr1Col2
        {
            get;
            set;
        }
        public ObservableCollection<StatisticsModel> _gr1Col1
        {
            get;
            set;
        }
        // gr 2 below
        public ObservableCollection<StatisticsModel> _gr2Col1
        {
            get;
            set;
        }
        public ObservableCollection<StatisticsModel> _gr2Col2
        {
            get;
            set;
        }
        // gr 3 below
        public ObservableCollection<StatisticsModel> _gr3Col1
        {
            get;
            set;
        }
        public ObservableCollection<StatisticsModel> _gr3Col2
        {
            get;
            set;
        }
        // gr 4 below
        public ObservableCollection<StatisticsModel> _gr4Col1
        {
            get;
            set;
        }
        public ObservableCollection<StatisticsModel> _gr4Col2
        {
            get;
            set;
        }
        // for pie charts
        public ObservableCollection<StatisticsModel> _gr2Par1 { get; set; }
        public ObservableCollection<StatisticsModel> _gr3Par1 { get; set; }

        public ObservableCollection<StatisticsModel> _gr4Par1 { get; set; }


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
