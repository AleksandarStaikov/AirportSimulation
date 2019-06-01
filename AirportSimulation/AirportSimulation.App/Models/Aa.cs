using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal class Aa : SingleCellBuildingComponent
    {
        public Aa(string nodeId, (int, int) cell) : base(BuildingComponentType.AA, nodeId, cell)
        {
        }

        //public override void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
