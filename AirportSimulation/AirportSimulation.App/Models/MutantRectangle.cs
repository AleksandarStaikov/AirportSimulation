using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AirportSimulation.App.Helpers;
using AirportSimulation.App.Infrastructure;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    public class MutantRectangle : Shape
    {
        public (int row, int column) Cell;
        public GridCell Content;

        public MutantRectangle((int, int) cell) : base()
        {
            Fill = new SolidColorBrush(Colors.Transparent);
            Cell = cell;

            Content = new EnabledCell(cell);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new RectangleGeometry(new Rect(new Point(5,2.5), new Size(60, 60)));
            }
        }

        public Grid GetGrid()
        {
            return Parent as Grid;
        }

        private void ContentClicked(MutantRectangle sender, BuildingComponentType type)
        {
            Content.ClickHandler(sender, type);
        }

        public void On_Click(BuildingComponentType type)
        {
            ContentClicked(this, type);
        }
    }
}
