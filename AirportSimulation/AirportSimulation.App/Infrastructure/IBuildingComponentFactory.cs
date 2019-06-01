namespace AirportSimulation.App.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using AirportSimulation.App.Models;


    internal interface IBuildingComponentFactory
    {
        GenericBuildingComponent CreateComponent(BuildingComponentType type, MutantRectangle container);
        
    }
}
