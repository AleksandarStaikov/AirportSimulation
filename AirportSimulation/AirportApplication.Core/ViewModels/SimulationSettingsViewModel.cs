namespace AirportSimulation.Core.ViewModels
{
    using AirportSimulation.Common.Models;
    using AirportSimulation.Core.Commands;
    using System.Windows.Input;

    public class SimulationSettingsViewModel
    {
        public SimulationSettingsViewModel()
        {
            this.BaggageTypeChange = new RelayCommand<SimulationSettingsViewModel>(this.HandleOnBaggageTypeChange);
        }


        public int CheckInStationsCount { get; set; }

        public int PscConveyorsCount { get; set; }

        public int PscInvalidationPercentage { get; set; }

        public int AscStaffCount { get; set; }

        //public int AscBagCheckingTime { get; set; }

        public int BsuCapacity { get; set; }

        public int BsuRobotsCount { get; set; }

        public int AaCount { get; set; }

        public int DistanceFromMpaToAa { get; set; }

        public int DistanceFromMpaToPickUp { get; set; }

        public int DistanceFromCheckInToPsc { get; set; }

        public int DistanceFromPscToMpa { get; set; }

        public int DistanceFromAscToMpa { get; set; }

        public int DistanceFromPscToAsc { get; set; }

        public int PickUpRate { get; set; }

        public int DropOffRate { get; set; }

        public int LoadUnloadRateAtAa { get; set; }

        public int EmployeesCount { get; set; }

        public int EmployeeProcessingBagTime { get; set; }

        public BaggageType BaggageType { get; set; } = BaggageType.Small;

        public bool IsSmallBaggageType => BaggageType == BaggageType.Small;

        public ICommand BaggageTypeChange { get; set; }

        private void HandleOnBaggageTypeChange(SimulationSettingsViewModel vm) =>
            this.BaggageType = vm.IsSmallBaggageType ? BaggageType.Small : BaggageType.Big;
    }
}
