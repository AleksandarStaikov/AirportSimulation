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
            var checkInToPsc = _chainLinkFactory.CreateConveyor(settings.ConveyorSettingsCheckInToPsc[0].Length);
            var psc = _chainLinkFactory.CreatePsc();
            var PscToMpa = _chainLinkFactory.CreateConveyor(settings.ConveyorSettingsPscToMpa[0].Length);
            var mpa = _chainLinkFactory.CreateMpa();
            var bsu = _chainLinkFactory.CreateBsu();
            var MpaToAA = _chainLinkFactory.CreateConveyor(settings.ConveyorSettingsMpaToAa[0].Length);
            var MpaToAA2 = _chainLinkFactory.CreateConveyor(settings.ConveyorSettingsMpaToAa[0].Length);
            var aa = _chainLinkFactory.CreateAa();
            var aa2 = _chainLinkFactory.CreateAa();

            aa.CurrentFlight = "FR6969";
            aa2.CurrentFlight = "sadsa2";



            //EndNodes
            var checkInDispatcher = _chainLinkFactory.CreateCheckInDispatcher();
            var bagCollector = _chainLinkFactory.CreateBagCollector();

            //Linking
            checkInDispatcher.SetCheckIns(new List<CheckInDesk>() { checkIn });
            checkIn.NextLink = checkInToPsc;
            checkInToPsc.NextLink = psc;
            psc.NextLink = PscToMpa;
            PscToMpa.NextLink = mpa;
            mpa.AddConnection(2, MpaToAA2);
            mpa.AddConnection(5, MpaToAA);
            bsu.NextLink = mpa;
            MpaToAA.NextLink = aa;
            MpaToAA2.NextLink = aa2;
            aa.NextLink = bagCollector;

            //Starting
            _timerService.RunNewTimer();
            checkInToPsc.Start();
            PscToMpa.Start();
            MpaToAA.Start();
            bsu.Start();
            checkInDispatcher.Start();
        }
    }
}
