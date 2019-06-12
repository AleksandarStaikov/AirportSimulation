namespace AirportSimulation.Common.Models
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;

    public class SimulationGridOptions : INotifyPropertyChanged
    {
        private bool _canBuildCheckIn = true;
        private bool _canBuildConveyor = true;
        private bool _canBuildManyToOneConveyor = true;
        private bool _canBuildBirdge = true;
        private bool _canBuildAsc = true;
        private bool _canBuildPsc = true;
        private bool _canBuildAa = true;
        private bool _canBuildPickUp = true;
        private bool _canBuildMpa = true;
        private bool _isGridEnabled = true;
        private bool _canCreate;
        private bool _canNext;

        public static int GRID_MAX_ROWS = 14;
        public static int GRID_MAX_COLUMNS = 19;

        public string GridRows => $"*#{++GRID_MAX_ROWS}";

        public string GridColumns => $"*#{++GRID_MAX_COLUMNS}";

        public bool CanBuildBridge
        {
            get => _canBuildBirdge;
            set
            {
                _canBuildBirdge = value;
                OnPropertyRaised(nameof(CanBuildBridge));
            }
        }

        public bool CanNext
        {
            get => _canNext;
            set
            {
                _canNext = value;
                OnPropertyRaised(nameof(CanNext));
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

        public void OnPropertyRaised(string propertyname) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
