using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulation.App.Helpers
{

    public class SeriesData
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ObservableCollection<StatisticsModel> Items { get; set; }

    }

    public class StatisticsModel : INotifyPropertyChanged
    {
          public string Category { get; set; }
        public string DisplayName { get; set; }

        public float _number = 0;
        public float Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Number"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
