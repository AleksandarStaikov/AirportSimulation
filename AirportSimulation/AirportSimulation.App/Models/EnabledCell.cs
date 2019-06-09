using AirportSimulation.App.Helpers;
using AirportSimulation.App.Infrastructure;
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
    class EnabledCell : GridCell, IClickable
    {
        public EnabledCell((int, int) cell) : base(cell)
        {
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if(type == BuildingComponentType.CheckIn)
            {
                var content = new SingleCellComponentFactory().CreateComponent(type, sender.Cell);
                ((CheckIn)content).PopulatePossibleNeighbours(sender);

                sender.ChangeContent(content);
            }
            
            
        }

        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            if(type != BuildingComponentType.CheckIn && type != BuildingComponentType.PSC)
            {
                var content = new DisabledCell(Cell);
                content.Fill = RectangleFactory.CreateDisabledRectangle().Fill;
                sender.ChangeContent(content);
            }
        }
    }
}
