namespace AirportSimulation.Common.Models
{
    using System;
    using System.Text;

    public class Flight : IFlight
    {
        public int BaggageCount { get; set; }

        public TimeSpan TimeToFlightSinceSimulationStart { get; set; } = new TimeSpan(0, 0, 0);

        public TimeSpan TimeToFlightPreparationSinceSimulationStart { get; set; } = new TimeSpan(0, 0, 0);

        public string FlightNumber { get; set; }

        public string Gate { get; set; }

        public string PickUpArea { get; set; }

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
