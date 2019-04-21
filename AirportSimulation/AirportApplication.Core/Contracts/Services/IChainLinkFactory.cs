namespace AirportSimulation.Core.Contracts.Services
{
    using Common.Models;
    using LinkNodes;

    public interface IChainLinkFactory
    {
        CheckInDesk CreateCheckInDesk();
        Psc CreatePsc();
        Asc CreateAsc();
        Mpa CreateMpa();
        BSU CreateBsu();
        Aa CreateAa();
        Conveyor CreateConveyor(int length);
        CheckInDispatcher CreateCheckInDispatcher();
        BagCollector CreateBagCollector();

        void SetSettings(SimulationSettings settings);
    }
}