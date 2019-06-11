namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using Helpers;
    using Common;
    
    internal class Mpa : SingleCellBuildingComponent, IParent
    {
        private static Mpa _instance = null;

        protected Mpa((int, int) cell) : base(BuildingComponentType.MPA, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.AA,
                BuildingComponentType.PA
            };
        }

        public static Mpa GetInstance((int, int) cell)
        {
            if(_instance == null)
            {
                _instance = new Mpa(cell)
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
                if (successor.AllowedNonConveyorSuccessors == null)
                {
                    var temp = successor as MultipleCellComponent;
                    temp.ChangeAllowedSuccessors(AllowedNonConveyorSuccessors);

                    if (temp is ManyToOneCell)
                    {
                        ((ManyToOneCell)temp).PredecessorType = this.Type;
                    }

                    NextNodes.Add(successor);
                }
                else
                {
                    if (successor is IParent parentComponent)
                    {
                        parentComponent.ChildClicked(this);
                    }
                }
            }

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
