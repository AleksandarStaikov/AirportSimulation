namespace AirportSimulation.Core.Services
{
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;
    using System.Data;
    using Common.Models.Contracts;

    public class ChainLinkFactory : IChainLinkFactory
    {
        private readonly CheckInDesk.Factory _checkInDeskFactory;
        private readonly Psc.Factory _pscFactory;
        private readonly Asc.Factory _ascFactory;
        private readonly Mpa.Factory _mpaFactory;
        private readonly BSU.Factory _bsuFactory;
        private readonly Aa.Factory _aaFactory;
        private readonly Conveyor.Factory _conveyorFactory;
        private readonly CheckInDispatcher.Factory _checkInDispatcherFactory;
        private readonly BagCollector.Factory _bagCollectorFactory;

        private SimulationSettings _simulationSettings;

        public ChainLinkFactory(CheckInDesk.Factory checkInDeskFactory,
            Psc.Factory pscFactory,
            Asc.Factory ascFactory,
            Mpa.Factory mpaFactory,
            BSU.Factory bsuFactory,
            Aa.Factory aaFactory,
            Conveyor.Factory conveyorFactory,
            CheckInDispatcher.Factory checkInDispatcherFactory,
            BagCollector.Factory bagCollectorFactory)
        {
            _checkInDeskFactory = checkInDeskFactory;
            _pscFactory = pscFactory;
            _ascFactory = ascFactory;
            _mpaFactory = mpaFactory;
            _bsuFactory = bsuFactory;
            _aaFactory = aaFactory;
            _conveyorFactory = conveyorFactory;
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

        public Conveyor CreateConveyor(int length)
        {
            ValidateSettings();
            return _conveyorFactory(length);
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
