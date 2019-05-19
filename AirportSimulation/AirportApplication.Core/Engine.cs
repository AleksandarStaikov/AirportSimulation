namespace AirportSimulation.Core
{
    using System.Collections.Generic;
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;
    using Contracts.Services;
    using LinkNodes;

    public class Engine : IEngine
    {
        private readonly IChainLinkFactory _chainLinkFactory;
        private readonly ITimerService _timerService;

        public Engine(IChainLinkFactory chainLinkFactory, ITimerService timerService)
        {
            _chainLinkFactory = chainLinkFactory;
            _timerService = timerService;
        }

        public void Run(SimulationSettings settings)
        {
            _chainLinkFactory.SetSettings(settings);
            _timerService.SetSettings(settings);

            var checkIn = _chainLinkFactory.CreateCheckInDesk();
            var checkInToConveyorConnector = _chainLinkFactory.CreateConveyorConnector();
            var checkInToPsc = _chainLinkFactory.CreateManyToOneConveyor(settings.ConveyorSettingsCheckInToPsc[0].Length);
            var psc = _chainLinkFactory.CreatePsc();
            var PscToMpa = _chainLinkFactory.CreateOneToOneConveyor(settings.ConveyorSettingsPscToMpa[0].Length);
            var PscToAsc = _chainLinkFactory.CreateOneToOneConveyor(settings.ConveyorSettingsPscToAsc[0].Length);
            var asc = _chainLinkFactory.CreateAsc();
            var ascToMpa = _chainLinkFactory.CreateOneToOneConveyor(settings.ConveyorSettingsAscToMpu[0].Length);
            var mpa = _chainLinkFactory.CreateMpa();
            var bsu = _chainLinkFactory.CreateBsu();
            var mpaToBsu = _chainLinkFactory.CreateOneToOneConveyor(10); //Implement conveyorSettings
            var bsuToMpa = _chainLinkFactory.CreateOneToOneConveyor(10); //Implement conveyorSettings
            var MpaToAA = _chainLinkFactory.CreateOneToOneConveyor(settings.ConveyorSettingsMpaToAa[0].Length);
            var aa = _chainLinkFactory.CreateAa();
            var pua = _chainLinkFactory.CreatePua();


            //EndNodes
            var checkInDispatcher = _chainLinkFactory.CreateCheckInDispatcher();
            var bagCollector = _chainLinkFactory.CreateBagCollector();

            //Linking
            checkInDispatcher.SetCheckIns(new List<CheckInDesk> { checkIn });

            checkIn.AddSuccessor(checkInToConveyorConnector);
            //checkIn1.AddSuccessor(checkIn1ToConveyorConnector);
            //checkIn2.AddSuccessor(checkIn2ToConveyorConnector);
            checkInToConveyorConnector.SetNextNode(checkInToPsc, 0);
            checkInToPsc.SetSuccessor(psc);
            //checkIn1ToConveyorConnector.SetNextNode(checkIn2ToPsc, 1);
            //checkIn2ToConveyorConnector.SetNextNode(checkIn2ToPsc, 2);

            // checkInToPsc.SetSuccessor(psc);



            //Transporting nodes
            mpaToBsu.SetSuccessor(bsu);
            PscToMpa.SetSuccessor(mpa);
            PscToAsc.SetSuccessor(asc);
            ascToMpa.SetSuccessor(mpa);
            MpaToAA.SetSuccessor(aa);
            bsuToMpa.SetSuccessor(mpa);

            //Processing and complex nodes
            psc.AddSuccessor(PscToMpa);
            psc.AddSuccessor(PscToAsc);
            asc.AddSuccessor(ascToMpa);
            asc.AddSuccessor(bagCollector);
            aa.AddSuccessor(bagCollector);
            mpa.AddSuccessor(MpaToAA);
            mpa.AddSuccessor(mpaToBsu);
            bsu.SetSuccessor(bsuToMpa);
            aa.AddSuccessor(bagCollector);



            //Starting
            _timerService.RunNewTimer();
            checkInToPsc.Start();

            PscToMpa.Start();
            mpa.Start();
            MpaToAA.Start();
            mpaToBsu.Start();
            bsuToMpa.Start();
            checkInDispatcher.Start();
        }

        public void RunDemo(SimulationSettings settings)
        {
            _chainLinkFactory.SetSettings(settings);
            _timerService.SetSettings(settings);

            var conveyorData = settings.ConveyorSettingsMpaToAa;

            var checkInDisp = _chainLinkFactory.CreateCheckInDispatcher();
            var checkIn = _chainLinkFactory.CreateCheckInDesk();
            var firstCheckInConnector = _chainLinkFactory.CreateConveyorConnector();
            var checkInToPsc = _chainLinkFactory.CreateManyToOneConveyor(conveyorData[0].Length);
            var psc = _chainLinkFactory.CreatePsc();
            var pscToMpa = _chainLinkFactory.CreateOneToOneConveyor(conveyorData[1].Length);
            var mpa = _chainLinkFactory.CreateMpa();
            var mpaToAA = _chainLinkFactory.CreateOneToOneConveyor(conveyorData[2].Length);
            var aa = _chainLinkFactory.CreateAa();
            var bagCollector = _chainLinkFactory.CreateBagCollector();

            checkInDisp.SetCheckIns(new List<CheckInDesk>() { checkIn });
            checkIn.AddSuccessor(firstCheckInConnector);
            firstCheckInConnector.SetNextNode(checkInToPsc, 0);
            checkInToPsc.SetSuccessor(psc);
            psc.AddSuccessor(pscToMpa);
            pscToMpa.SetSuccessor(mpa);

            aa.AddSuccessor(bagCollector);
            mpaToAA.SetSuccessor(aa);

            mpa.AddSuccessor(mpaToAA);

            checkInToPsc.Start();
            mpaToAA.Start();
            pscToMpa.Start();
			mpa.Start();

            _timerService.RunNewTimer();
            checkInDisp.Start();
        }
    }
}