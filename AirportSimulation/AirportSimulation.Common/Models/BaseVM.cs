using System.ComponentModel;

namespace AirportSimulation.Common.Models
{
    class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged.Invoke(this, propertyChangedEventArgs);
            }
        }
    }
}