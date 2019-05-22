using AirportSimulation.App.Helpers;
using AirportSimulation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AirportSimulation.App.Models
{
    class EnabledCell : GridCell
    {
        public EnabledCell((int, int) cell) : base(cell)
        {
        }

        public override void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            sender.Content = new CheckIn("asda", sender.Cell);
            sender.Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(type));
        }
    }
}
