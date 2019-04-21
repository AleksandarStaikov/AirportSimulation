namespace AirportSimulation.App
{
    using AirportSimulation.App.Views;
    using Core;
    using NLog;
    using System;
    using System.Windows;
    using Autofac;
    using Common.Models;
    using Core.Contracts;

    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Logger.Info("Starting application...");

                base.OnStartup(e);

                using (var container = ContainerConfig.Configure())
                {
                    container.BeginLifetimeScope();

                    var engine = container.Resolve<IEngine>();
                    //Pass the settings here
                    engine.Run(new SimulationSettings());
                }
                    
                InitializeMainWindow();

                Logger.Info("Application stared.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Something went wrong while starting the application, see inner exception", 
                    ex.InnerException?.Message);

                throw new Exception(ex.Message, ex);
            }
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            ContainerConfig.Stop();
        }

        private void InitializeMainWindow()
        {
            Current.MainWindow = new MainWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowState = WindowState.Maximized,
                ResizeMode = ResizeMode.CanMinimize,
                WindowStyle = WindowStyle.ThreeDBorderWindow,
            };

            Current.MainWindow.Closed += HandleClosed;
            Current.MainWindow.Show();
        }
    }
}
