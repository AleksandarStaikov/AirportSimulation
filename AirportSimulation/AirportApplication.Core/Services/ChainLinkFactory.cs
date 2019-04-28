namespace AirportSimulation.Core.Services
{
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;
    using System.Data;

    public class ChainLinkFactory : IChainLinkFactory
    {
        private readonly CheckInDesk.Factory _checkInDeskFactory;
        private readonly Psc.Factory _pscFactory;
        private readonly Asc.Factory _ascFactory;
        private readonly Mpa.Factory _mpaFactory;
        private readonly BSU.Factory _bsuFactory;
        private readonly Aa.Factory _aaFactory;
        private readonly OneToOneConveyor.Factory _oneToOneConveyorFactory;
        private readonly ManyToOneConveyor.Factory _manyToOneConveyorFactory;
        private readonly ConveyorConnector.Factory _conveyorConnectorFactory;
        private readonly CheckInDispatcher.Factory _checkInDispatcherFactory;
        private readonly BagCollector.Factory _bagCollectorFactory;

        private SimulationSettings _simulationSettings;

        public ChainLinkFactory(CheckInDesk.Factory checkInDeskFactory,
            Psc.Factory pscFactory,
            Asc.Factory ascFactory,
            Mpa.Factory mpaFactory,
            BSU.Factory bsuFactory,
            Aa.Factory aaFactory,
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
            _oneToOneConveyorFactory = oneToOneConveyorFactory;
            _manyToOneConveyorFactory = manyToOneConveyorFactory;
            _conveyorConnectorFactory = conveyorConnectorFactory;
            _checkInDispatcherFactory = checkInDispatcherFactory;
            _bagCollectorFactory = bagCollectorFactory;
        }

        public CheckInDesk CreateCheckInDesk()
        {
            ValidateSettings();
            return _checkInDeskFactory();
        }

        public Psc CreatePsc()
        {
            ValidateSettings();
            return _pscFactory();
        }

        public Asc CreateAsc()
        {
            ValidateSettings();
            return _ascFactory();
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
            return _aaFactory();
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
    }
}
