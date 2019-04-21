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
        List<Flight> Flights { get; set; }
    }
}
