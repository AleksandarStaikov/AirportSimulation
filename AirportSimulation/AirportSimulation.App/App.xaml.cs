using AirportApplication.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AirportSimulation.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IoC.Start();

            Current.MainWindow = new MainWindow
            {
                DataContext = IoC.Container
            };

            Current.MainWindow.Closed += HandleClosed;
            Current.MainWindow.Show();
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            IoC.Stop();
        }
    }
}
