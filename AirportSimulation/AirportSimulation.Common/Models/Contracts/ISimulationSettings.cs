namespace AirportSimulation.Common.Models.Contracts
{
    using System.Collections.Generic;

    public interface ISimulationSettings : IFlightManagement
    {
        IEnumerable<NodeCreationData> Nodes { get; set; }

        PscSettings Pscs { get; set; }
        AscSettings Ascs { get; set; }

        List<ConveyorSettings> ConveyorSettingsMpaToAa { get; set; }
        List<ConveyorSettings> ConveyorSettingsMpaToPickUp { get; set; }
        List<ConveyorSettings> ConveyorSettingsCheckInToPsc { get; set; }
        List<ConveyorSettings> ConveyorSettingsPscToMpa { get; set; }
        List<ConveyorSettings> ConveyorSettingsAscToMpu { get; set; }
        List<ConveyorSettings> ConveyorSettingsPscToAsc { get; set; }
    }
    public interface IMultiplierSettings
    {
        int Multiplier { get; set; }
    }

    public interface IFlightManagement : IMultiplierSettings
    {
        List<Flight> OutgoingFlights { get; set; }

        List<Flight> IncomingFlights { get; set; }

        int TransBaggagePercentage { get; set; }
    }

    public interface IPscSettings
    {
        int ProcessingRateInMilliseconds { get; set; }
        int PscInvalidationPercentage { get; set; }
    }

    public interface IAscSettings
    {
        int ProcessingRateInMilliseconds { get; set; }
        int AscInvalidationPercentage { get; set; }
    }
}
