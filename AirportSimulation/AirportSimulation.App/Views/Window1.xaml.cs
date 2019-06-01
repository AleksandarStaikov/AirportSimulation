namespace AirportSimulation.App.Views
{

    using Helpers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : UserControl
    {
        public Window1()
        {
            InitializeComponent();

            this.DataContext = new StatisticsViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformFromDevice;
            double dx = m.M11;
            double dy = m.M22;

            ScaleTransform s = (ScaleTransform)StatsGrid.LayoutTransform;
            s.ScaleX = 1 / dx;
            s.ScaleY = 1 / dy;


        }
    }
}
