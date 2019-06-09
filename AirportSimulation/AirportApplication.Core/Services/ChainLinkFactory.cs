namespace AirportSimulation.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting.Channels;
    using Abstractions.Core.Contracts;
    using Common;
    using Common.Models;
    using Contracts.Services;
    using LinkNodes;
    using LinkNodes.Contracts;

    public class ChainLinkFactory : IChainLinkFactory
    {
        public static List<IChainLink> Nodes = new List<IChainLink>();

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

        public IChainLink CreateChainLink(NodeCreationData nodeData, SimulationSettings settings)
        {
            switch (nodeData.Type)
            {
                case BuildingComponentType.CheckIn:
                    return _checkInDeskFactory.Invoke(nodeData.Id);
                case BuildingComponentType.PSC:
                    return _pscFactory.Invoke(settings.Pscs, nodeData.Id);
                case BuildingComponentType.ASC:
                    return _ascFactory.Invoke(settings.Ascs, nodeData.Id);
                case BuildingComponentType.MPA:
                    return _mpaFactory.Invoke(nodeData.Id);
                case BuildingComponentType.BSU:
                    return _bsuFactory.Invoke(nodeData.Id);
                case BuildingComponentType.AA:
                    return _aaFactory.Invoke(_gatesCount++, nodeData.Id);
                case BuildingComponentType.PA:
                    return _puaFactory.Invoke(_puasCount++, nodeData.Id);
                case BuildingComponentType.Conveyor:
                    return _oneToOneConveyorFactory.Invoke(nodeData.Length ?? 0, nodeData.Id);
                case BuildingComponentType.ManyToOneConveyor:
                    return _manyToOneConveyorFactory.Invoke(nodeData.Length ?? 0, nodeData.Id);
                default:
                    throw new ArgumentException("Unsupported node type");
            }
        }

        public ICheckInDesk CreateCheckInDesk()
        {
            ValidateSettings();
            var checkInDesk = _checkInDeskFactory(Guid.NewGuid().ToString());
            Nodes.Add(checkInDesk);
            return checkInDesk;
        }

        public IPsc CreatePsc()
        {
            ValidateSettings();
            var psc = _pscFactory(_simulationSettings.Pscs, Guid.NewGuid().ToString());
            Nodes.Add(psc);
            return psc;
        }

        public IAsc CreateAsc()
        {
            ValidateSettings();
            var asc = _ascFactory(_simulationSettings.Ascs, Guid.NewGuid().ToString());
            Nodes.Add(asc);
            return asc;
        }

        public IMpa CreateMpa()
        {
            ValidateSettings();
            var mpa = _mpaFactory(Guid.NewGuid().ToString());
            Nodes.Add(mpa);
            return mpa;
        }

        public IBSU CreateBsu()
        {
            ValidateSettings();
            var bsu = _bsuFactory(Guid.NewGuid().ToString());
            Nodes.Add(bsu);
            return bsu;
        }

        public IAa CreateAa()
        {
            ValidateSettings();
            var aa = _aaFactory(_gatesCount++, Guid.NewGuid().ToString());
            Nodes.Add(aa);
            return aa;
        }

        public IPickUpArea CreatePua()
        {
            ValidateSettings();
            var pickUpArea = _puaFactory(_puasCount++, Guid.NewGuid().ToString());
            Nodes.Add(pickUpArea);
            return pickUpArea;
        }

        public IOneToOneConveyor CreateOneToOneConveyor(int length)
        {
            ValidateSettings();
            var oneToOneConveyor = _oneToOneConveyorFactory(length, Guid.NewGuid().ToString());
            Nodes.Add(oneToOneConveyor);
            return oneToOneConveyor;
        }

        public IManyToOneConveyor CreateManyToOneConveyor(int length)
        {
            ValidateSettings();
            var manyToOneConveyor = _manyToOneConveyorFactory(length, Guid.NewGuid().ToString());
            Nodes.Add(manyToOneConveyor);
            return manyToOneConveyor;
        }

        public IConveyorConnector CreateConveyorConnector()
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

        public ICheckInDispatcher CreateCheckInDispatcher()
        {
            ValidateSettings();
            var checkInDispatcher = _checkInDispatcherFactory(_simulationSettings, Guid.NewGuid().ToString());
            Nodes.Add(checkInDispatcher);
            return checkInDispatcher;
        }

        public IAADispatcher CreateAaDispatcher()
        {
            ValidateSettings();
            var aaDispatcher = _aaDispatcherFactory(_simulationSettings, Guid.NewGuid().ToString());
            Nodes.Add(aaDispatcher);
            return aaDispatcher;
        }

        public IBagCollector CreateBagCollector()
        {
            ValidateSettings();
            var bagCollector = _bagCollectorFactory(Guid.NewGuid().ToString());
            Nodes.Add(bagCollector);
            return bagCollector;
        }

        #endregion
    }
}
