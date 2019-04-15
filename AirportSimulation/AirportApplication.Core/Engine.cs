namespace AirportSimulation.Core
{
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

        public void Run()
        {
            var checkIn = _chainLinkFactory.CreateCheckInDesk();
            var checkInToPsc = _chainLinkFactory.CreateConveyor(10);
            var psc = _chainLinkFactory.CreatePsc();
            var PscToMpa = _chainLinkFactory.CreateConveyor(10);
            var mpa = _chainLinkFactory.CreateMpa();
            var bsu = _chainLinkFactory.CreateBsu();
            var MpaToAA = _chainLinkFactory.CreateConveyor(10);
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

            _timerService.RunNewTimer(2);
            checkInToPsc.Start();
            PscToMpa.Start();
            MpaToAA.Start();
            bsu.Start();
            checkInDispatcher.DispatchBaggage();
        }
    }
}
