namespace AirportSimulation.App.Models
{
    internal interface ISucceedable
    {
        void PopulateAdjacentRectangles(MutantRectangle parentContainer);

        void ShowBlinkingCells();

        void HideBlinkingCells();
    }
}
