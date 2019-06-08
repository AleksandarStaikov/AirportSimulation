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
            else if(type == BuildingComponentType.ASC)
            {
                return CreateAsc(container.Cell);
            }
            else if(type == BuildingComponentType.MPA)
            {
                return Mpa.GetInstance(container.Cell);
            }
            else if(type == BuildingComponentType.AA)
            {
                container.ReadyToGoNext?.Invoke();

                return CreateAa(container.Cell);
            }
            else if(type == BuildingComponentType.PA)
            {
                return CreatePa(container.Cell);
            }

            return null;
        }

        private CheckIn CreateCheckIn(MutantRectangle container)
        {
            var temp = new CheckIn(container.Cell)
            {
                Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.CheckIn))
            };
            temp.PopulatePossibleNeighbours(container); //TODO: Move to EnabledCell
            return temp;
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
    }
}
