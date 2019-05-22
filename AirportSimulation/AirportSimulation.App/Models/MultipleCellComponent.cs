using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    class MultipleCellComponent : GenericBuildingComponent
    {
        public MultipleCellComponent(BuildingComponentType type, string nodeId, (int, int) cell) : base(type, nodeId, cell)
        {

        }
    }
}
