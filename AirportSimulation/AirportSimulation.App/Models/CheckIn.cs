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
    using System.Windows.Media;

    internal class CheckIn : SingleCellBuildingComponent, IParent
    {
        private Succeedable _succeedable;
        public CheckIn(string nodeId, (int, int) cell) : base(BuildingComponentType.CheckIn, nodeId, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.PSC,
            };

            _succeedable = new Succeedable(this);

            _succeedable.PopulateBlinkingCells();
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            if(type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor)
            {
                _succeedable.PopulateBlinkingCells();
            }
            else
            {
                _succeedable.HideBlinkingCells();
            }
        }
    }
}
