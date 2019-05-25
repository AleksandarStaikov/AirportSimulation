using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulation.App.Models
{
    interface ISucceedable
    {
        void PopulateAdjacentRectangles(MutantRectangle parentContainer);

        void ShowBlinkingCells();

        void HideBlinkingCells();
    }
}
