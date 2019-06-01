namespace AirportSimulation.App.Models
{
    using AirportSimulation.App.Models;
    using AirportSimulation.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    interface IParent
    {
        void ShowBlinkingChildren(BuildingComponentType type);

        void ChildClicked(GenericBuildingComponent successor);

        void PopulatePossibleNeighbours(MutantRectangle container);
    }
}
