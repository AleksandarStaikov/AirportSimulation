namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;

    public class Psc : ProcessingNode, IProcessingNode
    {
        public delegate Psc Factory(IPscSettings pscSettings);

        private readonly IPscSettings _pscSettings;
        private readonly Random _randomGen;

        public Psc(IPscSettings pscSettings, ITimerService timerService) : base(timerService)
        {
            _pscSettings = pscSettings;
            _randomGen = new Random();
        }

        public override void Process(Baggage baggage)
        {
            var isFail = _randomGen.Next(0, 101) < _pscSettings.PscInvalidationPercentage;

            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(_pscSettings.ProcessingRateInMilliseconds), $"Primary security check processing - passed: {isFail}");

            baggage.Destination = isFail ? typeof(Asc).Name : typeof(Mpa).Name;
        }
    }
}
