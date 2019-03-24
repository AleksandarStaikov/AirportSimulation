namespace AirportSimulation.Core
{
    using LinkNodes;
    using Services;

    public class Engine
    {
        public void Run()
        {
            var aa = new Aa(new TimerService());
            var mpaToAa = new Conveyor(10, new TimerService());
            var mpa = new Mpa(new TimerService());
            var pscToMpa = new Conveyor(10, new TimerService());
            var psc = new Psc(new TimerService());
            var checkInToPsc = new Conveyor(10, new TimerService());
            var checkIn = new CheckInDesk(new TimerService());

            checkIn.SuccessSuccessor = checkInToPsc;
            checkInToPsc.SuccessSuccessor = psc;
            psc.SuccessSuccessor = pscToMpa;
            pscToMpa.SuccessSuccessor = mpa;
            mpa.SuccessSuccessor = mpaToAa;

        }
    }
}
