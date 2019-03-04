namespace AirportSimulation.App
{
    using AirportApplication.Core;
    using NLog;
    using System;
    using System.Windows;

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
                    container.BeginLifetimeScope();

                Current.MainWindow = new MainWindow();
                Current.MainWindow.Closed += HandleClosed;
                Current.MainWindow.Show();

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
    }
}
