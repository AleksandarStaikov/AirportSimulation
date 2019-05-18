namespace AirportSimulation.Common.Models.Contracts
{
    using System.Collections.Generic;

    public interface ISimulationSettings : IFlightManagement
    {
        List<CheckInSettings> CheckIns { get; set; }
        int CheckInStationsCount { get; }
        List<PscSettings> Pscs { get; set; }
        int PscCount { get; }
        List<AscSettings> Ascs { get; set; }
        int AscCount { get; }
        BsuSettings Bsu { get; set; }
        List<AaSettings> Aas { get; set; }
        int AaCount { get; }
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
        int AscStaffCount { get; set; }
        int AscInvalidationPercentage { get; set; }
    }
}
