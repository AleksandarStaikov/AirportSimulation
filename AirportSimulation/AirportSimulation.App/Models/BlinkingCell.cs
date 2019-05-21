namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using System.Windows;

    class BlinkingCell : GridCell
    {
        public readonly GenericBuildingComponent ParentComponent;

        public BlinkingCell(GenericBuildingComponent parent, (int, int) cell) : base(cell)
        {
            ParentComponent = parent;
        }

        public void HideBlinkingCellBasedOnComponentType(BuildingComponentType selectedType)
        {
                        
        }


    }
}
