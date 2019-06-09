namespace AirportSimulation.Common.Models
{
    using Contracts;
    using System.Collections.Generic;

    public class SimulationSettings : ISimulationSettings
    {
        public SimulationSettings()
        {
            Pscs =  new PscSettings();
            Ascs = new AscSettings();

            ConveyorSettingsMpaToAa = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsMpaToPickUp = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsCheckInToPsc = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsPscToMpa = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsAscToMpu = new List<ConveyorSettings>() { new ConveyorSettings() };
            ConveyorSettingsPscToAsc = new List<ConveyorSettings>() { new ConveyorSettings() };

            OutgoingFlights = new List<Flight>();
            IncomingFlights = new List<Flight>();
        }

        public IEnumerable<NodeCreationData> Nodes { get; set; }

        #region NodesInfos

        public PscSettings Pscs { get; set; }

        public AscSettings Ascs { get; set; }
        
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

    public class PscSettings : IPscSettings
    {
        public int ProcessingRateInMilliseconds { get; set; } = 1000;

        public int PscInvalidationPercentage { get; set; }
    }

    public class AscSettings : IAscSettings
    {
        public int ProcessingRateInMilliseconds { get; set; } = 1000;
        
        public int AscInvalidationPercentage { get; set; }
    }

    public class ConveyorSettings
    {
        public int Length { get; set; } = 10;
    }
}
