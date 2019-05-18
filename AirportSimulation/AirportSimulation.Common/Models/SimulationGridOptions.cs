namespace AirportSimulation.Common.Models
{
    using System.ComponentModel;

    public class SimulationGridOptions : INotifyPropertyChanged
    {
        private bool _canBuildCheckIn;
		private bool _canBuildConveyor;
        private bool _canBuildManyToOneConveyor;
        private bool _canBuildAsc;
		private bool _canBuildPsc;
		private bool _canBuildAa;
        private bool _canBuildPickUp;
		private bool _canBuildMpa;
        private bool _isGridEnabled = true;
        private bool _canCreate;
        private bool _canClear;
		private bool _canRun;

        public int GRID_MAX_ROWS = 14;
        public int GRID_MAX_COLUMNS = 19;

        public string GridRows => $"*#{++GRID_MAX_ROWS}";

        public string GridColumns => $"*#{++GRID_MAX_COLUMNS}";

        public bool CanRun
        {
            get => _canRun;
            set
            {
                _canRun = value;
                OnPropertyRaised(nameof(CanRun));
            }
        }

        public bool IsGridEnabled
        {
            get => _isGridEnabled;
            set
            {
                _isGridEnabled = value;
                OnPropertyRaised(nameof(IsGridEnabled));
            }
        }

        public bool CanClear
        {
            get => _canClear;
            set
            {
                _canClear = value;
                OnPropertyRaised(nameof(CanClear));
            }
        }

        public bool CanCreate
        {
            get => _canCreate;
            set
            {
                _canCreate = value;
                OnPropertyRaised(nameof(CanCreate));
            }
        }

		public bool CanBuildMpa
		{
			get => _canBuildMpa;
			set
			{
				_canBuildMpa = value;
				OnPropertyRaised(nameof(CanBuildMpa));
			}
		}

		public bool CanBuildCheckIn
        {
            get => _canBuildCheckIn;
            set
            {
                _canBuildCheckIn = value;
                OnPropertyRaised(nameof(CanBuildCheckIn));
            }
        }

        public bool CanBuildConveyor
        {
            get => _canBuildConveyor;
            set
            {
                _canBuildConveyor = value;
                OnPropertyRaised(nameof(CanBuildConveyor));
            }
        }

        public bool CanBuildManyToOneConveyor
        {
            get => _canBuildManyToOneConveyor;
            set
            {
                _canBuildManyToOneConveyor = value;
                OnPropertyRaised(nameof(CanBuildManyToOneConveyor));
            }
        }

        public bool CanBuildPsc
        {
            get => _canBuildPsc;
            set
            {
                _canBuildPsc = value;
                OnPropertyRaised(nameof(CanBuildPsc));
            }
        }
        
        public bool CanBuildAsc
        {
            get => _canBuildAsc;
            set
            {
                _canBuildAsc = value;
                OnPropertyRaised(nameof(CanBuildAsc));
            }
        }

        public bool CanBuildAa
        {
            get => _canBuildAa;
            set
            {
                _canBuildAa = value;
                OnPropertyRaised(nameof(CanBuildAa));
            }
        }

        public bool CanBuildPickUp
        {
            get => _canBuildPickUp;
            set
            {
                _canBuildPickUp = value;
                OnPropertyRaised(nameof(CanBuildPickUp));
            }
        }

        private void OnPropertyRaised(string propertyname) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
