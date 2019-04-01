using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirportSimulation.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Swap between different grids implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ButtonHover_Click(object sender, RoutedEventArgs e)
        {
            double index = double.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(113 + (150 * index), 38, 0, 0);
            

            switch (index)
            {
                case 0:
                    GridAdministration.Visibility = Visibility.Hidden;
                    break;
                case 2.35:
                    GridAdministration.Visibility = Visibility.Visible;
                    break;
                case 4.70:
                    GridAdministration.Visibility = Visibility.Hidden;
                    break;

            }
        }

        /// <summary>
        /// Close Program Button Implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Frequently Asked Questions Button Implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ButtonFaq_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }
    }
}
