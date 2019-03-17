namespace AirportSimulation.Core
{
    using System.Runtime.InteropServices;
    using LinkNodes;

    public class Engine
    {
        public void Run()
        {
            var aa = new Aa();
            var mpaToAa = new Conveyor(10);
            var mpa = new Mpa();
            var pscToMpa = new Conveyor(10);
            var psc = new Psc();
            var checkInToPsc = new Conveyor(10);
            var checkIn = new CheckInDesk();

            checkIn.SuccessSuccessor = checkInToPsc;
            checkInToPsc.SuccessSuccessor = psc;
            psc.SuccessSuccessor = pscToMpa;
            pscToMpa.SuccessSuccessor = mpa;
            mpa.SuccessSuccessor = mpaToAa;

        }
    }
}
