namespace AirportSimulation.Core.LinkNodes
{
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;
    using System;

    public class Psc : ProcessingNode, IProcessingNode, IPsc
    {
        public delegate Psc Factory(IPscSettings pscSettings, string nodeId);

        private readonly IPscSettings _pscSettings;
        private readonly Random _randomGen;

        public Psc(IPscSettings pscSettings, string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
            _pscSettings = pscSettings;
            _randomGen = new Random();
        }

        public override void Process(IBaggage baggage)
        {
            var isFail = _randomGen.Next(0, 101) < _pscSettings.PscInvalidationPercentage;

            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                TimerService.ConvertMillisecondsToTimeSpan(_pscSettings.ProcessingRateInMilliseconds),
                $"Primary security check ID-{NodeId} processing - { (isFail ? LoggingConstants.PscCheckFailed : LoggingConstants.PscCheckSucceeded)}");

            baggage.Destination = isFail ? typeof(Asc).Name : typeof(Mpa).Name;
        }
    }
}
