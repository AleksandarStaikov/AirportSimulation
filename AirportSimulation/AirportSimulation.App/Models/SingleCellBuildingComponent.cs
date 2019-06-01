using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AirportSimulation.App.Infrastructure;
using AirportSimulation.Common;
using AirportSimulation.Common.Models;

namespace AirportSimulation.App.Models
{
    internal abstract class SingleCellBuildingComponent : GenericBuildingComponent
    {
        public GenericBuildingComponent Predecessor { get; set; }

        public SingleCellBuildingComponent(BuildingComponentType type, string nodeId, (int, int) cell) 
            : base(type, nodeId, cell)
        {
            
        }
    }
}
