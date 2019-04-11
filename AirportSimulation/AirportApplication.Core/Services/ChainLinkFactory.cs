namespace AirportSimulation.Core.Services
{
    using Contracts;
    using Contracts.Services;
    using LinkNodes;

    public class ChainLinkFactory : IChainLinkFactory
    {
        private readonly CheckInDesk.Factory _checkInDeskFactory;
        private readonly Psc.Factory _pscFactory;
        private readonly Asc.Factory _ascFactory;
        private readonly Mpa.Factory _mpaFactory;
        private readonly Aa.Factory _aaFactory;
        private readonly Conveyor.Factory _conveyorFactory;
        private readonly CheckInDispatcher.Factory _checkInDispatcherFactory;
        private readonly BagCollector.Factory _bagCollectorFactory;

        public ChainLinkFactory(CheckInDesk.Factory checkInDeskFactory,
            Psc.Factory pscFactory,
            Asc.Factory ascFactory,
            Mpa.Factory mpaFactory,
            Aa.Factory aaFactory,
            Conveyor.Factory conveyorFactory,
            CheckInDispatcher.Factory checkInDispatcherFactory,
            BagCollector.Factory bagCollectorFactory)
        {
            _checkInDeskFactory = checkInDeskFactory;
            _pscFactory = pscFactory;
            _ascFactory = ascFactory;
            _mpaFactory = mpaFactory;
            _aaFactory = aaFactory;
            _conveyorFactory = conveyorFactory;
            _checkInDispatcherFactory = checkInDispatcherFactory;
            _bagCollectorFactory = bagCollectorFactory;
        }

        public CheckInDesk CreateCheckInDesk()
        {
            return _checkInDeskFactory();
        }

        public Psc CreatePsc()
        {
            return _pscFactory();
        }

        public Asc CreateAsc()
        {
            return _ascFactory();
        }

        public Mpa CreateMpa()
        {
            return _mpaFactory();
        }

        public Aa CreateAa()
        {
            return _aaFactory();
        }

        public Conveyor CreateConveyor(int length)
        {
            return _conveyorFactory(length);
        }

        #region EndNodes

        public CheckInDispatcher CreateCheckInDispatcher()
        {
            return _checkInDispatcherFactory();
        }

        public BagCollector CreateBagCollector()
        {
            return _bagCollectorFactory();
        }

        #endregion
    }
}
