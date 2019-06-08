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
        public GenericBuildingComponent CreateComponent(BuildingComponentType type, MutantRectangle container)
        {
            if(type == BuildingComponentType.Conveyor)
            {
                return CreateOneToOneConveyor(container);
            }
            else if(type == BuildingComponentType.ManyToOneConveyor)
            {
                return CreateManyToOneConveyor(container);
            }
            return null;
        }

        private OneToOneCell CreateOneToOneConveyor(MutantRectangle container)
        {
            var temp = new OneToOneCell(container.Cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.Conveyor))
            };

            return temp;
        }

        private ManyToOneCell CreateManyToOneConveyor(MutantRectangle container)
        {
            var temp = new ManyToOneCell(container.Cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.ManyToOneConveyor))
            };

            return temp;
        } 
    }
}
