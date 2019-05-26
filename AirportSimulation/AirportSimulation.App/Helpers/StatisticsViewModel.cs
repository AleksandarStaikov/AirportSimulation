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

            _gr1Col1.Add(new StatisticsModel() { Category = "Collected Bags", Number = Convert.ToInt32(statisticsData?.FirstCollectedBag?.Log?.FirstOrDefault()) });
            _gr1Col1.Add(new StatisticsModel() { Category = "Dispatched Bags", Number = Convert.ToInt32(statisticsData?.LastCollectedBag?.Log?.FirstOrDefault()) });

            _gr1Col2.Add(new StatisticsModel() { Category = "Collected Bags", Number = Convert.ToInt32(statisticsData?.FirstDispatchedBag?.Log?.FirstOrDefault()) });
            _gr1Col2.Add(new StatisticsModel() { Category = "Dispatched Bags", Number = Convert.ToInt32(statisticsData?.LastDispatchedBag?.Log?.FirstOrDefault()) });

            _gr2Par1.Add(new StatisticsModel() { Category = "PSC Failed Percentage", Number = statisticsData?.PscFailedBags?.Count ?? 0 });
            _gr2Par1.Add(new StatisticsModel() { Category = "PSC Succeeded Percentage", Number = statisticsData?.PscFailedBags?.Count ?? 0 });

            _gr3Par1.Add(new StatisticsModel() { Category = "ASC Failed Percentage", Number = statisticsData?.AscFailedBags?.Count ?? 0 });
            _gr3Par1.Add(new StatisticsModel() { Category = "ASC Succeeded Percentage", Number = statisticsData?.AscFailedBags?.Count ?? 0 });

            Series.Add(new SeriesData() { DisplayName = "First", Items = _gr1Col1 });
            Series.Add(new SeriesData() { DisplayName = "Last", Items = _gr1Col2 });
            Series.Add(new SeriesData() { DisplayName = "Pie Chart", Items = _gr2Par1 });
            Series.Add(new SeriesData() { DisplayName = "Pie Second", Items = _gr3Par1 });


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


        public ObservableCollection<StatisticsModel> _gr2Par1 { get; set; }
        public ObservableCollection<StatisticsModel> _gr3Par1 { get; set; }

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
