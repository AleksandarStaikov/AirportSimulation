namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;

    internal interface IClickable
    {
        void ClickHandler(MutantRectangle sender, BuildingComponentType type);

        void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type);
    }
}
