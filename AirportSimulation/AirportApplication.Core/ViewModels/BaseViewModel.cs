namespace AirportSimulation.Core.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Media;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel() : 
            this (10, 10, Colors.Black)
        {
        }

        public BaseViewModel(int width, int height, Color color)
        {
            this.Width = width;
            this.Height = height;
            this.Color = color;
        }

        public Color Color { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

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
