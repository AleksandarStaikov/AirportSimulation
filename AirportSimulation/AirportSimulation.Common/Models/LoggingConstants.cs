namespace AirportSimulation.Common.Models
{
    public static class LoggingConstants
    {
        public static string PscCheckFailed => "PcsValidationFailed";
        public static string PscCheckSucceeded => "PcsCheckSucceeded";
        public static string AscCheckFailed => "AscCheckFailed";
        public static string AscCheckSucceeded => "AscCheckSucceeded";

        public static string BagArrivedLateAtAirportArea => "Bag is delaying the flight";
        public static string BagArrivedOnTimeAtAirportArea => "Bag bag arrived on time for the flight";

        public static string BagRedirectedToAnotherFlight => "Bag sent to be transferred to another flight";

        public static string BagReceivedInTemplate = "Bag received in {0}";

        public static string ReceivedInRobotSendingTo = "Robot processing. Sending to {0}";
    }
}
