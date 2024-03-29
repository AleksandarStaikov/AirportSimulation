﻿namespace AirportSimulation.Core.LinkNodes
{
    using System;
    using Abstractions.Contracts;
    using Abstractions.Core;
    using Abstractions.Core.Contracts;
    using Common.Models;
    using Common.Models.Contracts;
    using Contracts;

    public class Asc : ProcessingNode, IProcessingNode, IAsc
    {
        public delegate Asc Factory(IAscSettings ascSettings, string nodeId);

        private readonly IAscSettings _ascSettings;
        private readonly Random _randomGen;

        public Asc(IAscSettings ascSettings, string nodeId, ITimerService timerService)
            : base(nodeId, timerService)
        {
            _ascSettings = ascSettings;
            _randomGen = new Random();
        }

        public override void Process(IBaggage baggage)
        {
            var isFail = _randomGen.Next(0, 101) < _ascSettings.AscInvalidationPercentage;

            baggage.AddEventLog(TimerService.GetTimeSinceSimulationStart(),
                TimerService.ConvertMillisecondsToTimeSpan(_ascSettings.ProcessingRateInMilliseconds),
                $"Advanced security check ID-{NodeId} processing - { (isFail ? LoggingConstants.AscCheckFailed : LoggingConstants.AscCheckSucceeded) }");

            _currentBaggage.Destination = isFail ? typeof(BagCollector).Name : typeof(Mpa).Name;
        }
    }
}