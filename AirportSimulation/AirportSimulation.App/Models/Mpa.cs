using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AirportSimulation.App.Helpers;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal class Mpa : SingleCellBuildingComponent, IParent
    {
        private static Mpa _instance = null;

        protected Mpa(string nodeId, (int, int) cell) : base(BuildingComponentType.MPA, nodeId, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.AA
            };
        }

        public static Mpa GetInstance(string nodeId, (int, int) cell)
        {
            if(_instance == null)
            {
                _instance = new Mpa(nodeId, cell)
                {
                    Fill = new ImageBrush(BuildingComponentsHelper.GetBuildingComponentImage(BuildingComponentType.MPA)),
                };
                _instance.successorEnabler = new Succeedable(_instance);
            }

            _instance.Cell = cell;

            return _instance;
        }

        public void ChildClicked(GenericBuildingComponent successor)
        {
            if (successor.GetType().BaseType == typeof(MultipleCellComponent))
            {
                var temp = successor as MultipleCellComponent;
                temp.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);
                if (temp is ManyToOneCell)
                {
                    ((ManyToOneCell)temp).PredecessorType = this.Type;
                }
            }
            NextNodes.Add(successor);

            ShowBlinkingChildren(successor.Type);
        }

        public void PopulatePossibleNeighbours(MutantRectangle container)
        {
            successorEnabler.PopulateAdjacentRectangles(container);
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            if(type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor || type == _instance.Type)
            {
                successorEnabler.ShowBlinkingCells();
            }
            else
            {
                successorEnabler.HideBlinkingCells();
            }
        }
    }
}
