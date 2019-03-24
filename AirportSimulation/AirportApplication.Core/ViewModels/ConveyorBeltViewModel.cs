namespace AirportSimulation.Core.ViewModels
{
    using AirportSimulation.Core.Commands;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class ConveyorBeltViewModel
    {
        private ObservableCollection<BaggageViewModel> _baggageCollection;


        public ConveyorBeltViewModel()
        {
            this._baggageCollection = new ObservableCollection<BaggageViewModel>();
            this.ProcessNewBaggage = new RelayCommand<BaggageViewModel>(this.HandleProcessNewBaggage);
        }

        public ICommand ProcessNewBaggage { get; set; }

        private void HandleProcessNewBaggage(BaggageViewModel vm)
        {
            if (vm != null)
                _baggageCollection.Add(vm);
        }
    }
}
