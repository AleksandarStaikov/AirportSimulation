using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal class Psc : SingleCellBuildingComponent, IParent
    {
        private List<BuildingComponentType> _allowedComponents
            = new List<BuildingComponentType>()
            {
                BuildingComponentType.ASC, BuildingComponentType.MPA
            };

        public Psc(string nodeId, (int, int) cell) : base(BuildingComponentType.PSC, nodeId, cell)
        {
            successorEnabler = new Succeedable(this);
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            throw new NotImplementedException();
        }
    }
}
