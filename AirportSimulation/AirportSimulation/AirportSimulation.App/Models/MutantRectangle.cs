namespace AirportSimulation.App.Models
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using AirportSimulation.Common;
    
    internal class MutantRectangle : Shape
    {
        public (int row, int column) Cell;
        public GridCell Content;
        public Action ReadyToGoNext;

        public MutantRectangle()
        {

        }

        public MutantRectangle((int, int) cell) : base()
        {
            Fill = new SolidColorBrush(Colors.Transparent);
            Cell = cell;

            Content = new EnabledCell(cell);
        }

        protected override Geometry DefiningGeometry => new RectangleGeometry(new Rect(new Point(5, 2.5), new Size(60, 60)));

        public Grid GetGrid() => Parent as Grid;

        private void ContentClicked(MutantRectangle sender, BuildingComponentType type)
        {
            if (Content is IClickable)
            {
                ((IClickable)Content).ClickHandler(sender, type);
            }
        }

        public void On_Click(BuildingComponentType type) => ContentClicked(this, type);

        public void ChangeContent(GridCell content)
        {
            Content = content;
            Fill = content.Fill;
        }

        public void On_BuildingComponentClicked(BuildingComponentType type)
        {
            if (Content is IClickable)
            {
                ((IClickable)Content).ComponentSelectedHandler(this, type);
            }
        }
    }
}
