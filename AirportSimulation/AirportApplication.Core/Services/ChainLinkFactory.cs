namespace AirportSimulation.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;

    public class ChainLinkFactory : IChainLinkFactory
    {
        private static List<IChainLink> Nodes = new List<IChainLink>();

        private readonly Aa.Factory _aaFactory;
        private readonly PickUpArea.Factory _puaFactory;
        private readonly Asc.Factory _ascFactory;
        private readonly BagCollector.Factory _bagCollectorFactory;
        private readonly BSU.Factory _bsuFactory;
        private readonly CheckInDesk.Factory _checkInDeskFactory;
        private readonly CheckInDispatcher.Factory _checkInDispatcherFactory;
        private readonly AADispatcher.Factory _aaDispatcherFactory;
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
            AADispatcher.Factory aaDispatcherFactory,
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
            _aaDispatcherFactory = aaDispatcherFactory;
            _bagCollectorFactory = bagCollectorFactory;

            _puasCount = 1;
            _gatesCount = 1;
        }

        public CheckInDesk CreateCheckInDesk()
        {
            ValidateSettings();
            var checkInDesk = _checkInDeskFactory(Guid.NewGuid().ToString());
            Nodes.Add(checkInDesk);
            return checkInDesk;
        }

        public Psc CreatePsc()
        {
            ValidateSettings();
            //TODO: Deal with indexes
            var psc = _pscFactory(_simulationSettings.Pscs[0], Guid.NewGuid().ToString());
            Nodes.Add(psc);
            return psc;
        }

        public Asc CreateAsc()
        {
            ValidateSettings();
            //TODO: Deal with indexes
            var asc = _ascFactory(_simulationSettings.Ascs[0], Guid.NewGuid().ToString());
            Nodes.Add(asc);
            return asc;
        }

        public Mpa CreateMpa()
        {
            ValidateSettings();
            var mpa = _mpaFactory(Guid.NewGuid().ToString());
            Nodes.Add(mpa);
            return mpa;
        }

        public BSU CreateBsu()
        {
            ValidateSettings();
            var bsu = _bsuFactory(Guid.NewGuid().ToString());
            Nodes.Add(bsu);
            return bsu;
        }

        public Aa CreateAa()
        {
            ValidateSettings();
            var aa = _aaFactory(_gatesCount++, Guid.NewGuid().ToString());
            Nodes.Add(aa);
            return aa;
        }

        public PickUpArea CreatePua()
        {
            ValidateSettings();
            var pickUpArea = _puaFactory(_puasCount++, Guid.NewGuid().ToString());
            Nodes.Add(pickUpArea);
            return pickUpArea;
        }

        public OneToOneConveyor CreateOneToOneConveyor(int length)
        {
            ValidateSettings();
            var oneToOneConveyor = _oneToOneConveyorFactory(length, Guid.NewGuid().ToString());
            Nodes.Add(oneToOneConveyor);
            return oneToOneConveyor;
        }

        public ManyToOneConveyor CreateManyToOneConveyor(int length)
        {
            ValidateSettings();
            var manyToOneConveyor = _manyToOneConveyorFactory(length, Guid.NewGuid().ToString());
            Nodes.Add(manyToOneConveyor);
            return manyToOneConveyor;
        }

        public ConveyorConnector CreateConveyorConnector()
        {
            ValidateSettings();
            var conveyorConnector = _conveyorConnectorFactory(Guid.NewGuid().ToString());
            Nodes.Add(conveyorConnector);
            return conveyorConnector;
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
            var checkInDispatcher = _checkInDispatcherFactory(_simulationSettings, Guid.NewGuid().ToString());
            Nodes.Add(checkInDispatcher);
            return checkInDispatcher;
        }

        public AADispatcher CreateAaDispatcher()
        {
            ValidateSettings();
            var aaDispatcher = _aaDispatcherFactory(_simulationSettings, Guid.NewGuid().ToString());
            Nodes.Add(aaDispatcher);
            return aaDispatcher;
        }

        public BagCollector CreateBagCollector()
        {
            ValidateSettings();
            var bagCollector = _bagCollectorFactory(Guid.NewGuid().ToString());
            Nodes.Add(bagCollector);
            return bagCollector;
        }

        #endregion
    }
}
