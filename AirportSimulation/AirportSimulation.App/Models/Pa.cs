using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal class Pa : SingleCellBuildingComponent
    {
        public Pa(string nodeId, (int, int) cell) : base(BuildingComponentType.PA, nodeId, cell)
        {
        }

    }
}
