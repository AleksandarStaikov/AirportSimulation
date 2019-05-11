namespace AirportSimulation.Core.Services
{
    using System.Data;
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;

    public class ChainLinkFactory : IChainLinkFactory
    {
        private readonly Aa.Factory _aaFactory;
        private readonly PickUpArea.Factory _puaFactory;
        private readonly Asc.Factory _ascFactory;
        private readonly BagCollector.Factory _bagCollectorFactory;
        private readonly BSU.Factory _bsuFactory;
        private readonly CheckInDesk.Factory _checkInDeskFactory;
        private readonly CheckInDispatcher.Factory _checkInDispatcherFactory;
        private readonly ConveyorConnector.Factory _conveyorConnectorFactory;
        private readonly ManyToOneConveyor.Factory _manyToOneConveyorFactory;
        private readonly Mpa.Factory _mpaFactory;
        private readonly OneToOneConveyor.Factory _oneToOneConveyorFactory;
        private readonly Psc.Factory _pscFactory;
        private int _puasCount;
        private int _gatesCount;

        private SimulationSettings _simulationSettings;

        public ChainLinkFactory(CheckInDesk.Factory checkInDeskFactory,
            Psc.Factory pscFactory,
            Asc.Factory ascFactory,
            Mpa.Factory mpaFactory,
            BSU.Factory bsuFactory,
            Aa.Factory aaFactory,
            PickUpArea.Factory puaFactory,
            OneToOneConveyor.Factory oneToOneConveyorFactory,
            ManyToOneConveyor.Factory manyToOneConveyorFactory,
            ConveyorConnector.Factory conveyorConnectorFactory,
            CheckInDispatcher.Factory checkInDispatcherFactory,
            BagCollector.Factory bagCollectorFactory)
        {
            _checkInDeskFactory = checkInDeskFactory;
            _pscFactory = pscFactory;
            _ascFactory = ascFactory;
            _mpaFactory = mpaFactory;
            _bsuFactory = bsuFactory;
            _aaFactory = aaFactory;
            _puaFactory = puaFactory;
            _oneToOneConveyorFactory = oneToOneConveyorFactory;
            _manyToOneConveyorFactory = manyToOneConveyorFactory;
            _conveyorConnectorFactory = conveyorConnectorFactory;
            _checkInDispatcherFactory = checkInDispatcherFactory;
            _bagCollectorFactory = bagCollectorFactory;

            _puasCount = 1;
            _gatesCount = 1;
        }

        public CheckInDesk CreateCheckInDesk()
        {
            ValidateSettings();
            return _checkInDeskFactory();
        }

        public Psc CreatePsc()
        {
            ValidateSettings();
            //TODO: Deal with indexes
            return _pscFactory(_simulationSettings.Pscs[0]);
        }

        public Asc CreateAsc()
        {
            ValidateSettings();
            //TODO: Deal with indexes
            return _ascFactory(_simulationSettings.Ascs[0]);
        }

        public Mpa CreateMpa()
        {
            ValidateSettings();
            return _mpaFactory();
        }

        public BSU CreateBsu()
        {
            ValidateSettings();
            return _bsuFactory();
        }

        public Aa CreateAa()
        {
            ValidateSettings();
            return _aaFactory(_gatesCount++);
        }

        public PickUpArea CreatePua()
        {

            ValidateSettings();
            return _puaFactory(_puasCount++);
        }

        public OneToOneConveyor CreateOneToOneConveyor(int length)
        {
            ValidateSettings();
            return _oneToOneConveyorFactory(length);
        }

        public ManyToOneConveyor CreateManyToOneConveyor(int length)
        {
            ValidateSettings();
            return _manyToOneConveyorFactory(length);
        }

        public ConveyorConnector CreateConveyorConnector()
        {
            ValidateSettings();
            return _conveyorConnectorFactory();
        }

        public void SetSettings(SimulationSettings settings)
        {
            this._simulationSettings = settings;
        }

        private void ValidateSettings()
        {
            if (_simulationSettings == null)
            {
                throw new NoNullAllowedException("The simulation settings have not been set");
            }
        }

        #region EndNodes

        public CheckInDispatcher CreateCheckInDispatcher()
        {
            ValidateSettings();
            return _checkInDispatcherFactory(_simulationSettings);
        }

        public BagCollector CreateBagCollector()
        {
            ValidateSettings();
            return _bagCollectorFactory();
        }

        #endregion
    }
}
