using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AirportSimulation.App.Helpers;
using AirportSimulation.App.Models;
using AirportSimulation.Common;

namespace AirportSimulation.App.Infrastructure
{
    class MultipleCellComponentFactory : IBuildingComponentFactory
    {
        public GenericBuildingComponent CreateComponent(BuildingComponentType type, (int, int) cell)
        {
            if(type == BuildingComponentType.Conveyor)
            {
                return CreateOneToOneConveyor(cell);
            }
            else if(type == BuildingComponentType.ManyToOneConveyor)
            {
                return CreateManyToOneConveyor(cell);
            }
            return null;
        }

        private OneToOneCell CreateOneToOneConveyor((int, int) cell)
        {
            var temp = new OneToOneCell(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.Conveyor))
            };

            return temp;
        }

        private ManyToOneCell CreateManyToOneConveyor((int, int) cell)
        {
            var temp = new ManyToOneCell(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.ManyToOneConveyor))
            };

            return temp;
        } 
    }
}
