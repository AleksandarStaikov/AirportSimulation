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

namespace AirportSimulation.App.Views
{
    /// <summary>
    /// Interaction logic for SimulationView.xaml
    /// </summary>
    public partial class SimulationView : UserControl
    {
        public SimulationView()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int selectedColumnIndex = -1, selectedRowIndex = -1;
            var grid = sender as Grid;
            if (grid != null)
            {
                var pos = e.GetPosition(grid);
                var temp = pos.X;
                for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
                {
                    var colDef = grid.ColumnDefinitions[i];
                    temp -= colDef.ActualWidth;
                    if (temp <= -1)
                    {
                        selectedColumnIndex = i;
                        break;
                    }
                }

                temp = pos.Y;
                for (var i = 0; i < grid.RowDefinitions.Count; i++)
                {
                    var rowDef = grid.RowDefinitions[i];
                    temp -= rowDef.ActualHeight;
                    if (temp <= -1)
                    {
                        selectedRowIndex = i;
                        break;
                    }
                }
            }

            MessageBox.Show(selectedColumnIndex + ", " + selectedRowIndex);
        }
    }
}
