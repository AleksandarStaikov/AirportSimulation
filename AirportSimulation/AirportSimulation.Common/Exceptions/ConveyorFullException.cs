namespace AirportSimulation.Common.Exceptions
{
    using System;

    public class ConveyorFullException : Exception
    {
        public ConveyorFullException()
        {

        }

        public ConveyorFullException(string message)
            :base(message)
        {

        }
    }
}
