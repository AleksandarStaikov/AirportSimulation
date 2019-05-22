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

        public GenericBuildingComponent CreateComponent(BuildingComponentType type, (int, int) cell)
        {
            if(type == BuildingComponentType.CheckIn)
            {
                return CreateCheckIn(cell);
            }
            else if(type == BuildingComponentType.PSC)
            {
                return CreatePsc(cell);
            }

            return null;
        }

        private CheckIn CreateCheckIn((int, int) cell)
        {
            return new CheckIn("asdas", cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn))
            };
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
