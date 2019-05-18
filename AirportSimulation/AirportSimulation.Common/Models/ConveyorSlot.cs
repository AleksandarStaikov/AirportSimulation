namespace AirportSimulation.Common.Models
{
    public class ConveyorSlot
    {
		public int SlotIndex { get; set; }

		// TODO: Uncomment when implemeting Many to one coveyors

		// public int? CheckInIndex { get; set; }

		// public int? PscIndex { get; set; }

		// public int? MpaIndex  { get; set; }

		// public int? AaIndex { get; set; }

		public ConveyorSlot NextSlot { get; set; }
	}
}
