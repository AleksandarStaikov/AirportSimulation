using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Abstractions.Contracts;
using AirportSimulation.Abstractions.Core;
using AirportSimulation.Abstractions.Core.Contracts;
using AirportSimulation.Common.Models;

namespace AirportSimulation.Core.LinkNodes
{
    public class Robot : ProcessingNode, IProcessingNode
    {
        public Robot(ITimerService timerService) : base(timerService)
        {
        }

        public override void Process(Baggage baggage)
        {
            baggage.AddEventLog(TimerService.ConvertMillisecondsToTimeSpan(1000), "Robot processing. Sending to " + 
                (baggage.Destination == typeof(Mpa).Name ? baggage.Destination : "BaggageBucket #" + baggage.Destination));
        }

        
    }
}
