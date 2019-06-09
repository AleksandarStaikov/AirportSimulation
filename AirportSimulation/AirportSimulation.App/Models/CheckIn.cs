namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using AirportSimulation.Common;
    using AirportSimulation.App.Helpers;

    internal class CheckIn : SingleCellBuildingComponent, IParent
    {
        public CheckIn((int, int) cell) : base(BuildingComponentType.CheckIn, cell)
        {
            AllowedNonConveyorSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.PSC,
            };

            successorEnabler = new Succeedable(this);
            NextNodes = new List<GenericBuildingComponent>(1);

            ConvertToSettingsService.StartNodes.Add(this);
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
            successorEnabler.PopulateAdjacentRectangles(container);
        }

        public void ShowBlinkingChildren(BuildingComponentType type)
        {
            if ((type == BuildingComponentType.Conveyor || type == BuildingComponentType.ManyToOneConveyor) 
                && NextNodes.Capacity != NextNodes.Count)
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
