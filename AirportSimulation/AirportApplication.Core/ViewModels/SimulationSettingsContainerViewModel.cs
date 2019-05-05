namespace AirportSimulation.Core.ViewModels
{
    using AirportSimulation.Common.Models;
    using AirportSimulation.Core.Commands;
    using AirportSimulation.Core.Contracts;
    using System.Windows.Input;

    public class SimulationSettingsContainerViewModel
    {

        public SimulationSettingsContainerViewModel()
        {
            CollectSettings = new RelayCommand<SimulationSettings>(InitializeSettings);
            SimulationSettingsViewModel = new SimulationSettings();
        }

        private void InitializeSettings(SimulationSettings obj)
        {
            var engine = ContainerConfig.Resolve<IEngine>();
            engine.Run(SimulationSettingsViewModel);
        }

        public SimulationSettings SimulationSettingsViewModel { get; set; }

        public ICommand CollectSettings { get; set; }
    }
}
