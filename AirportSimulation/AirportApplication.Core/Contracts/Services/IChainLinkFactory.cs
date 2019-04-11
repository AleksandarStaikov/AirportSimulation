namespace AirportSimulation.Core.Contracts.Services
{
    using LinkNodes;

    public interface IChainLinkFactory
    {
        CheckInDesk CreateCheckInDesk();
        Psc CreatePsc();
        Asc CreateAsc();
        Mpa CreateMpa();
        Aa CreateAa();
        Conveyor CreateConveyor(int length);
        CheckInDispatcher CreateCheckInDispatcher();
        BagCollector CreateBagCollector();
    }
}