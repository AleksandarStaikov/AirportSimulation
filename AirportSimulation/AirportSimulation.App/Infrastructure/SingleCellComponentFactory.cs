namespace AirportSimulation.App.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using AirportSimulation.App.Models;
    using AirportSimulation.App.Helpers;
    using System.Windows.Media;

    internal class SingleCellComponentFactory : IBuildingComponentFactory
    {
        public SingleCellComponentFactory()
        {
            
        }

        public GenericBuildingComponent CreateComponent(BuildingComponentType type, MutantRectangle container)
        {
            if(type == BuildingComponentType.CheckIn)
            {
                return CreateCheckIn(container);
            }
            else if(type == BuildingComponentType.PSC)
            {
                return CreatePsc(container.Cell);
            }

            return null;
        }

        private CheckIn CreateCheckIn(MutantRectangle container)
        {
            var temp = new CheckIn("asdas", container.Cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn))
            };

            temp.PopulatePossibleNeighbours(container);

            return temp;
        } 

        private Psc CreatePsc((int, int) cell)
        {
            return new Psc("asda", cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.PSC))
            };
        }
    }
}
