using System;

namespace AirportSimulation.Common.Models
{
    public class SimulationSettings
    {
        public int CheckInStationsCount { get; set; }

        public int PscConveyorsCount { get; set; }

        public int PscInvalidationPercentage { get; set; }

        public int AscStaffCount { get; set; }

        //public int AscBagCheckingTime { get; set; }

        public int BsuCapacity { get; set; }

        public int BsuRobotsCount { get; set; }

        public int AaCount { get; set; }

        public int DistanceFromMpaToAa { get; set; }

        public int DistanceFromMpaToPickUp { get; set; }

        public int DistanceFromCheckInToPsc { get; set; }

        public int DistanceFromPscToMpu { get; set; }

        public int DistanceFromAscToMpu { get; set; }

        public int DistanceFromPscToAsc { get; set; }

        public int PickUpRate { get; set; }

        public int DropOffRate { get; set; }

        public static TimeSpan TimeToFlight = new TimeSpan(0, 2, 0);
    }
}
