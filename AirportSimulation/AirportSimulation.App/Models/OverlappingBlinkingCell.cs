namespace AirportSimulation.App.Models
{
    using System.Collections.Generic;
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.Common;

    internal class OverlappingBlinkingCell : GridCell, IClickable
    {
        private readonly List<BlinkingCell> Layers;

        public OverlappingBlinkingCell((int, int) cell) : base(cell)
        {
            Layers = new List<BlinkingCell>();
            Fill = RectangleFactory.CreateBlinkingRectangle().Fill;
        }

        public void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            foreach(BlinkingCell blinkingCell in Layers)
            {
                if(!(sender.Content is GenericBuildingComponent))
                {
                    blinkingCell.ClickHandler(sender, type);
                }
                else
                {
                    blinkingCell.ParentComponent.ChildClicked(sender.Content as GenericBuildingComponent);
                    //Layers.Remove(blinkingCell);
                }
            }
        }

        public void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type)
        {
            foreach(BlinkingCell blinkingCell in Layers)
            {
                blinkingCell.ComponentSelectedHandler(sender, type);
            }
        }

        public void AddLayer(BlinkingCell layer)
        {
            if(layer.ParentComponent is MultipleCellComponent)
            {
                Layers.Insert(0, layer);
            }
            else
            {
                Layers.Add(layer);
            }
        }
    }
}
