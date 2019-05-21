﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal class Pa : SingleCellBuildingComponent
    {
        public Pa(BuildingComponentType type, string nodeId, (int, int) cell) : base(type, nodeId, cell)
        {
        }
    }
}
