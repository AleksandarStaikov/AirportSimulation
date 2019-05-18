﻿namespace AirportSimulation.Core
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
            var checkInToPsc =
                _chainLinkFactory.CreateManyToOneConveyor(settings.ConveyorSettingsCheckInToPsc[0].Length);
            var psc = _chainLinkFactory.CreatePsc();
            var PscToMpa = _chainLinkFactory.CreateOneToOneConveyor(settings.ConveyorSettingsPscToMpa[0].Length);
            var mpa = _chainLinkFactory.CreateMpa();
            var bsu = _chainLinkFactory.CreateBsu();
            var mpaToBsu = _chainLinkFactory.CreateOneToOneConveyor(10); //Implement conveyorSettings
            var bsuToMpa = _chainLinkFactory.CreateOneToOneConveyor(10); //Implement conveyorSettings
            var MpaToAA = _chainLinkFactory.CreateOneToOneConveyor(settings.ConveyorSettingsMpaToAa[0].Length);
            var aa = _chainLinkFactory.CreateAa();


            //EndNodes
            var checkInDispatcher = _chainLinkFactory.CreateCheckInDispatcher();
            var bagCollector = _chainLinkFactory.CreateBagCollector();

            //Linking
            checkInDispatcher.SetCheckIns(new List<CheckInDesk> {checkIn});

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
            MpaToAA.SetSuccessor(aa);
            bsuToMpa.SetSuccessor(mpa);

            //Processing and complex nodes
            psc.AddSuccessor(PscToMpa);
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
    }
}