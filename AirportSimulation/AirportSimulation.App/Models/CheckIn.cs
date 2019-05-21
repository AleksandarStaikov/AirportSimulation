namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.Common.Models;
    using AirportSimulation.App.Helpers;

    internal class CheckIn : SingleCellBuildingComponent
    {
        private List<BuildingComponentType> _allowedComponents
            = new List<BuildingComponentType>()
            {
                BuildingComponentType.PSC,
            };

        public CheckIn(BuildingComponentType type, string nodeId, (int, int) cell) : base(type, nodeId, cell)
        {
            successorEnabler = new Succeedable(this);
        }

        
    }
}
