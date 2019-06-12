namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;

    internal interface IParent
    {
        void ShowBlinkingChildren(BuildingComponentType type);

        void ChildClicked(GenericBuildingComponent successor);

        void PopulatePossibleNeighbours(MutantRectangle container);
    }
}
