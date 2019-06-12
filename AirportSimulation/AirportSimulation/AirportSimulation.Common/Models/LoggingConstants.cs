namespace AirportSimulation.Common.Models
{
    public static class LoggingConstants
    {
        public static string PscCheckFailed => "PcsValidationFailed";
        public static string PscCheckSucceeded => "PcsCheckSucceeded";
        public static string AscCheckFailed => "AscCheckFailed";
        public static string AscCheckSucceeded => "AscCheckSucceeded";

        public static string BagArrivedLateAtAirportArea => "Bag is delaying the flight";
        public static string BagArrivedOnTimeAtAirportArea => "Bag arrived on time for the flight";

        public static string BagRedirectedToAnotherFlight => "Bag sent to be transferred to another flight";

        public static string BagReceivedText = "Bag received in";
        public static string BagTransporterIdText = "TransporterId";
        public static string BagReceivedInTemplate = BagReceivedText + " {0}, " + BagTransporterIdText + " {1}";

        public static string ReceivedInRobotSendingTo = "Robot processing. Sending to {0}";
    }
}
