namespace AirportSimulation.Common.Models
{
    using Contracts;
    using System.Collections.Generic;

    public class SimulationSettings : ISimulationSettings
    {
        public SimulationSettings()
        {
            CheckIns = new List<CheckInSettings>() { new CheckInSettings() };
            Pscs = new List<PscSettings>() { new PscSettings() };
            Ascs = new List<AscSettings>() { new AscSettings() };
            Bsu = new BsuSettings();
            Aas = new List<AaSettings>() { new AaSettings() };

            ConveyorSettingsMpaToAa = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsMpaToPickUp = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsCheckInToPsc = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsPscToMpa = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsAscToMpu = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsPscToAsc = new List<ConveyorSettings>() { new ConveyorSettings() };

            OutgoingFlights = new List<Flight>() { new Flight(){} };
            OutgoingFlights = new List<Flight>() { new Flight()
            {
                Gate = "A2",
                FlightState = FlightState.Incoming
            } };
        }

        #region NodesInfos

        public List<CheckInSettings> CheckIns { get; set; }
        public int CheckInStationsCount => CheckIns.Count;

        public List<PscSettings> Pscs { get; set; }
        public int PscCount => Pscs.Count;

        public List<AscSettings> Ascs { get; set; }
        public int AscCount => Ascs.Count;

        public BsuSettings Bsu { get; set; }

        public List<AaSettings> Aas { get; set; }
        public int AaCount => Aas.Count;


        public List<ConveyorSettings> ConveyorSettingsMpaToAa { get; set; }

        public List<ConveyorSettings> ConveyorSettingsMpaToPickUp { get; set; }

        public List<ConveyorSettings> ConveyorSettingsCheckInToPsc { get; set; }

        public List<ConveyorSettings> ConveyorSettingsPscToMpa { get; set; }

        public List<ConveyorSettings> ConveyorSettingsAscToMpu { get; set; }

        public List<ConveyorSettings> ConveyorSettingsPscToAsc { get; set; }

        #endregion

        public List<Flight> OutgoingFlights { get; set; }

        public List<Flight> IncomingFlights { get; set; }

        public int TransBaggagePercentage { get; set; }

        public int Multiplier { get; set; } = 1;
    }

    public class CheckInSettings
    {
        public int ProcessingRateInMilliseconds { get; set; } = 1000;
    }

    public class PscSettings : IPscSettings
    {
        public int ProcessingRateInMilliseconds { get; set; } = 1000;

        public int PscInvalidationPercentage { get; set; }
    }

    public class AscSettings : IAscSettings
    {
        public int ProcessingRateInMilliseconds { get; set; } = 1000;
        public int AscStaffCount { get; set; }
        public int AscInvalidationPercentage { get; set; }
    }

    public class AaSettings
    {
        public int PickUpRate { get; set; }
    }

    public class BsuSettings
    {
        public int BsuCapacity { get; set; } = 100;
        public int BsuRobotsCount { get; set; } = 2;
        public int MinutesToFlightDispatchTime { get; set; } = 20;
    }

    public class ConveyorSettings
    {
        public int Length { get; set; } = 10;
        
        public int SpeedInMilliseconds { get; set; } = 1000;
    }
}
