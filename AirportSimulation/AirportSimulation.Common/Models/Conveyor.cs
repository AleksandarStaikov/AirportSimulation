using System.Collections.Generic;

namespace AirportSimulation.Common.Models
{
    public class Conveyor
    {
        public List<KeyValuePair<int, int>> Cells { get; set; }

        public GridNode Prev { get; set; }

        public GridNode Next { get; set; }
    }
}
