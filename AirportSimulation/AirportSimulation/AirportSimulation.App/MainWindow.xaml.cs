﻿namespace AirportSimulation.App
{
    using AirportSimulation.App.Helpers;
    using AirportSimulation.App.Models;
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using AirportSimulation.Core;
    using AirportSimulation.Core.Contracts;
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnNextButton_Command = new RelayCommand<MainWindow>(SwitchTab);
            OnRunButton_Command = new RelayCommand<MainWindow>(RunSimulation);
        }

        private void RunSimulation(MainWindow obj)
        {
            var engine = ContainerConfig.Resolve<IEngine>();

            var settings = new SimulationSettings
            {
                IncomingFlights = FlightsOrganizer.IncomingFlights.ToList(),
                OutgoingFlights = FlightsOrganizer.OutgoingFlights.ToList(),
                Multiplier = FlightsOrganizer.Multiplier,
                Ascs = FlightsOrganizer.CurrentAscSettings,
                Pscs = FlightsOrganizer.CurrentPscSettings,
                TransBaggagePercentage = FlightsOrganizer.TransBaggagePercentage,
                Nodes = ConvertToSettingsService.Convert()
            };

            engine.ActualRun(settings);
            StatisticsViewModel.StartStatisticsTimer();
        }

        private void SwitchTab(MainWindow obj)
        {
            var a = ConvertToSettingsService.Convert();
            FlightsView.IncomingPickUpAreasComboBox.ItemsSource = ConvertToSettingsService.GetAvailablePickUpAreas();
            FlightsView.OutgoingGatesComboBox.ItemsSource = ConvertToSettingsService.GetAvailableGates();
            FlightsView.IncomingGatesComboBox.ItemsSource = ConvertToSettingsService.GetAvailableGates();

            DisableBuildingComponentsButtons();

            this.MainTabMenu.SelectedIndex = 1;
        }

        public ICommand OnRunButton_Command { get; set; }

        public ICommand OnNextButton_Command { get; set; }

        private void DisableBuildingComponentsButtons()
        {
            SimulationView.CheckIn.IsEnabled = false;
            SimulationView.Conveyor.IsEnabled = false;
            SimulationView.ManyToOneConveyor.IsEnabled = false;
            SimulationView.PSC.IsEnabled = false;
            SimulationView.ASC.IsEnabled = false;
            SimulationView.AA.IsEnabled = false;
            SimulationView.PA.IsEnabled = false;
            SimulationView.MPA.IsEnabled = false;
            SimulationView.Next.IsEnabled = false;
            SimulationView.Export.IsEnabled = false;
            SimulationView.Import.IsEnabled = false;
            SimulationView.ClearGridButton.IsEnabled = false;
        }
    }
}
