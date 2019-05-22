using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.App.Models;
using AirportSimulation.Common;

namespace AirportSimulation.App.Models
{
    internal interface IClickable
    {
        void ClickHandler(MutantRectangle sender, BuildingComponentType type);

        void ComponentSelectedHandler(MutantRectangle sender, BuildingComponentType type);
    }
}
