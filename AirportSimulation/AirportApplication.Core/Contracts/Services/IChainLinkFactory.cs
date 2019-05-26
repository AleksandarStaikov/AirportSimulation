namespace AirportSimulation.Core.Contracts.Services
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using LinkNodes;

    public interface IChainLinkFactory
    {
        IChainLink CreateChainLink(NodeCreationData nodeData, SimulationSettings settings);
        CheckInDesk CreateCheckInDesk();
        Psc CreatePsc();
        Asc CreateAsc();
        Mpa CreateMpa();
        BSU CreateBsu();
        Aa CreateAa();
        PickUpArea CreatePua();
        OneToOneConveyor CreateOneToOneConveyor(int length);
        ManyToOneConveyor CreateManyToOneConveyor(int length);
        ConveyorConnector CreateConveyorConnector();
        CheckInDispatcher CreateCheckInDispatcher();
        AADispatcher CreateAaDispatcher();
        BagCollector CreateBagCollector();

        void SetSettings(SimulationSettings settings);
    }
}