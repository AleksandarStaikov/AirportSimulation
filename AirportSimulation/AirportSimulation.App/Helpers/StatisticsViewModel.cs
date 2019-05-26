namespace AirportSimulation.App.Helpers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using AirportSimulation.Core.Services;


    class StatisticsViewModel : INotifyPropertyChanged
    {
        public StatisticsData statisticsData = new StatisticsData();

        public StatisticsViewModel()
        {

            Series = new List<SeriesData>();

            Warnings = new ObservableCollection<StatisticsModel>();
            Errors = new ObservableCollection<StatisticsModel>();

            Errors.Add(new StatisticsModel { Category = "Testing", Number = statisticsData?.PscFailedBags?.Count ?? 0 });
            Errors.Add(new StatisticsModel { Category = "Testing", Number = statisticsData?.PscFailedBags?.Count ?? 0});
            Errors.Add(new StatisticsModel { Category = "Testing", Number = statisticsData?.PscFailedBags?.Count ?? 0 });
            Errors.Add(new StatisticsModel { Category = "Testing", Number = 44 });

            Warnings.Add(new StatisticsModel { Category = "Testing", Number = 44 });
            Warnings.Add(new StatisticsModel { Category = "Testing", Number = 44 });
            Warnings.Add(new StatisticsModel { Category = "Testing", Number = 44 });
            Warnings.Add(new StatisticsModel { Category = "Testing", Number = 44 });


            Series.Add(new SeriesData() { DisplayName = "Test1", Items = Errors });
            Series.Add(new SeriesData { DisplayName = "Test1333", Items = Warnings });


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

        public ObservableCollection<StatisticsModel> Warnings
        {
            get;
            set;
        }

        public ObservableCollection<StatisticsModel> Errors
        {
            get;
            set;
        }



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
