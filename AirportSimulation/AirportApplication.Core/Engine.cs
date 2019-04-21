namespace AirportSimulation.Core
{
    using Abstractions.Contracts;
    using Common.Models;
    using Contracts;
    using Contracts.Services;

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
            var aa = _chainLinkFactory.CreateAa();


            //EndNodes
            var checkInDispatcher = _chainLinkFactory.CreateCheckInDispatcher();
            var bagCollector = _chainLinkFactory.CreateBagCollector();

            //Linking
            checkInDispatcher.NextLink = checkIn;
            checkIn.NextLink = checkInToPsc;
            checkInToPsc.NextLink = psc;
            psc.NextLink = PscToMpa;
            PscToMpa.NextLink = mpa;
            mpa.NextLink.Add(bsu);
            mpa.NextLink.Add(MpaToAA);
            bsu.NextLink = mpa;
            MpaToAA.NextLink = aa;
            aa.NextLink = bagCollector;

            //Starting
            _timerService.RunNewTimer();
            checkInToPsc.Start();
            PscToMpa.Start();
            MpaToAA.Start();
            bsu.Start();
            checkInDispatcher.DispatchBaggage();
        }
    }
}
