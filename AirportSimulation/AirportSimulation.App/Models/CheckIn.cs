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
        private ISucceedable _succeedable;
        public CheckIn(string nodeId, (int, int) cell) : base(BuildingComponentType.CheckIn, nodeId, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.PSC,
            };

            _succeedable = new Succeedable(this);
            NextNodes = new List<GenericBuildingComponent>(1);
        }

        public void ChildClicked(GenericBuildingComponent successor)
        {
            if(successor.GetType().BaseType == typeof(MultipleCellComponent))
            {
                var temp = successor as MultipleCellComponent;
                temp.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                if(temp is ManyToOneCell)
                {
                    ((ManyToOneCell)temp).PredecessorType = this.Type;
                }
            }
            NextNodes.Add(successor);

            ShowBlinkingChildren(successor.Type);
        }

        public void PopulatePossibleNeighbours(MutantRectangle container)
        {
            _succeedable.PopulateAdjacentRectangles(container);
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            if ((type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor) 
                && NextNodes.Capacity != NextNodes.Count)
            {
                _succeedable.ShowBlinkingCells();
            }
            else
            {
                _succeedable.HideBlinkingCells();
            }
        }
    }
}
