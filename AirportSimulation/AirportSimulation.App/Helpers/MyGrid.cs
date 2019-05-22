using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AirportSimulation.App.Helpers
{
    public class MyGrid : Grid
    {
        public MyGrid()
        {

        }

        public void RemoveVisualChild(int index)
        {
            RemoveLogicalChild(Children[index]);

            RemoveVisualChild(Children[index]);
            
        }
    }
}
