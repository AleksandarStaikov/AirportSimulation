namespace AirportSimulation.App
{
    using Core;
    using NLog;
    using System;
    using System.Windows;
	using Views;

	public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Logger.Info("Starting application...");

				ContainerConfig.Configure();
                    
                InitializeMainWindow();

                Logger.Info("Application stared.");
            }
            catch (Exception ex)
            {
                Logger.Error("Something went wrong while starting the application, see inner exception", 
                    ex.InnerException?.Message);

                throw new Exception(ex.Message, ex);
            }

			base.OnStartup(e);
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
