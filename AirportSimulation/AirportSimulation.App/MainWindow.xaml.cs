namespace AirportSimulation.App
{
    using AirportSimulation.App.Helpers;
    using AirportSimulation.App.Models;
    using AirportSimulation.Common;
    using System;
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnNextButton_Command = new RelayCommand<MainWindow>(SwitchTab);
        }

        private void SwitchTab(MainWindow obj)
        {
            var a = ConvertToSettingsService.Convert();
            FlightsView.PickUpAreasComboBox.ItemsSource = ConvertToSettingsService.GetAvailablePickUpAreas();
            FlightsView.GatesComboBox.ItemsSource = ConvertToSettingsService.GetAvailableGates();

            this.MainTabMenu.SelectedIndex = 1;
        }

        public ICommand OnNextButton_Command { get; set; }
    }
}
