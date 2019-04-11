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

        public void Run()
        {
            _timerService.RunNewTimer(1);
            //var aa = _chainLinkFactory.CreateAa();
            //var mpaToAa = _chainLinkFactory.CreateConveyor(10);
            //var mpa = _chainLinkFactory.CreateMpa();
            //var asc = _chainLinkFactory.CreateAsc();
            //var pscToMpa = _chainLinkFactory.CreateConveyor(10);
           // var pscToAsc = _chainLinkFactory.CreateConveyor(10);
            //var psc = _chainLinkFactory.CreatePsc();
            var checkInToPsc = _chainLinkFactory.CreateConveyor(10);
            var checkIn = _chainLinkFactory.CreateCheckInDesk();

            //EndNodes
            var checkInDispatcher = _chainLinkFactory.CreateCheckInDispatcher();
            var bagCollector = _chainLinkFactory.CreateBagCollector();

            //Linking
            checkInDispatcher.NextLink = checkIn;
            checkIn.NextLink = checkInToPsc;
            checkInToPsc.NextLink = bagCollector;
            //checkInToPsc.SuccessSuccessor = psc;
            //psc.SuccessSuccessor = pscToMpa;
            //psc.FailSuccessor = pscToAsc;
            //pscToMpa.SuccessSuccessor = mpa;
            //pscToAsc.SuccessSuccessor = asc;
            //mpa.SuccessSuccessor = mpaToAa;
            //mpaToAa.SuccessSuccessor = aa;

//asc.SuccessSuccessor = bagCollector;
            //aa.SuccessSuccessor = bagCollector;
        }
    }
}
