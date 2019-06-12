namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;

    internal class OneToOneCell : MultipleCellComponent //TODO: Disable previous blinking cells
    {
        public OneToOneCell((int, int) cell) : base(BuildingComponentType.Conveyor, cell)
        {
        }

        public override void ShowBlinkingChildren(BuildingComponentType type)
        {
            if ((AllowedNonConveyorSuccessors.Contains(type) || type == this.Type) && NextNodes.Count == 0) 
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
