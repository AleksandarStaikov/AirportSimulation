using AirportSimulation.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulation.App.Models
{
    interface ICreatable
    {
        NodeCreationData GetCreationData();
    }
}
