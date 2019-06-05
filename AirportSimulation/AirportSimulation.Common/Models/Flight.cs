namespace AirportSimulation.Common.Models
{
    using System;
    using System.Text;

    public class Flight
    {
        public int BaggageCount { get; set; } = 100;

        public TimeSpan TimeToFlightSinceSimulationStart { get; set; } = new TimeSpan(0, 2, 0);

        public TimeSpan TimeToFlightPreparationSinceSimulationStart { get; } = new TimeSpan(0, 1, 0);

        public string FlightNumber { get; set; } = "FR6969";

        public string Gate { get; set; } = "A1";

        public string PickUpArea { get; set; } = "P1";

        public int DispatchedBaggageCount { get; set; } = 0;

        public FlightState FlightState { get; set; } = FlightState.WaitingForPreparation;

        public string FlightStateDescription => ToFormattedText(FlightState);

        private string ToFormattedText(FlightState value)
        {
            var stringVal = value.ToString();
            var bld = new StringBuilder();

            for (var i = 0; i < stringVal.Length; i++)
            {
                if (char.IsUpper(stringVal[i]))
                {
                    bld.Append(" ");
                }

                bld.Append(stringVal[i]);
            }

            return bld.ToString();
        }
    }

    public enum FlightState
    {
        WaitingForPreparation,
        InPreparation,
        Departed,
        Landed,
        Incoming
    }
}
