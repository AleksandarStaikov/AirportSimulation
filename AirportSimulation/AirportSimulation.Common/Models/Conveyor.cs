namespace AirportSimulation.Common.Models
{
    using System.Collections.Generic;

    public class Conveyor
    {
        public List<(int, int)> Cells { get; set; }

        public GridNode Prev { get; set; }

        public GridNode Next { get; set; }
    }
}
