namespace AirportSimulation.Common.Models
{
	using System.Collections.Generic;

	public class ConveyorBelt
	{
		public List<ConveyorSlot> ConveyorSlots { get; set; } = new List<ConveyorSlot>();
	}
}
