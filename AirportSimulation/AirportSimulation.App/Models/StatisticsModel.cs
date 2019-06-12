namespace AirportSimulation.App.Models
{
	using System.Collections.ObjectModel;
	using System.ComponentModel;

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
			get => _number;
			set
			{
				_number = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Number"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
