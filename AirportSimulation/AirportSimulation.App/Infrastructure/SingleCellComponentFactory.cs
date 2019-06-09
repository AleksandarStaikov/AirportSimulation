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
            else if(type == BuildingComponentType.ASC)
            {
                return CreateAsc(cell);
            }
            else if(type == BuildingComponentType.MPA)
            {
                return Mpa.GetInstance(cell);
            }
            else if(type == BuildingComponentType.AA)
            {
                return CreateAa(cell);
            }
            else if(type == BuildingComponentType.PA)
            {
                return CreatePa(cell);
            }
            else if(type == BuildingComponentType.Bridge){
                return CreateBridge(cell);
            }

            return null;
        }

        private CheckIn CreateCheckIn((int, int) cell)
        {
            return new CheckIn(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn))
            };
            
        } 

        private Psc CreatePsc((int, int) cell)
        {
            return new Psc(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.PSC))
            };
        }

        private Asc CreateAsc((int, int) cell)
        {
            return new Asc(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.ASC))
            };
        }

        private Aa CreateAa((int, int) cell)
        {
            return new Aa(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.AA))
            };
        }

        private Pa CreatePa((int, int) cell)
        {
            return new Pa(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.PA))
            };
        }

        private ConveyorBridge CreateBridge((int, int) cell)
        {
            return new ConveyorBridge(cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.Bridge))
            };
        }
    }
}
