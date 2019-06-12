namespace AirportSimulation.Core.Contracts.Services
{
    using Abstractions.Core.Contracts;
    using Common.Models;
    using LinkNodes;
    using LinkNodes.Contracts;

    public interface IChainLinkFactory
    {
        IChainLink CreateChainLink(NodeCreationData nodeData, SimulationSettings settings);
        ICheckInDesk CreateCheckInDesk();
        IPsc CreatePsc();
        IAsc CreateAsc();
        IMpa CreateMpa();
        IBSU CreateBsu();
        IAa CreateAa();
        IPickUpArea CreatePua();
        IOneToOneConveyor CreateOneToOneConveyor(int length);
        IManyToOneConveyor CreateManyToOneConveyor(int length);
        IConveyorConnector CreateConveyorConnector();
        ICheckInDispatcher CreateCheckInDispatcher();
        IAADispatcher CreateAaDispatcher();
        IBagCollector CreateBagCollector();

        void SetSettings(SimulationSettings settings);
    }
}