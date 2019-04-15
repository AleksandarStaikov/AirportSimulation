using AirportSimulation.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulation.Common.ViewModels
{
    public class AdminViewModel
    {
        
        ObservableCollection<SimulationSettings> SimulationSettings
        {
            get;
            set;
        }

        public void InjectVariables()
        {
            ObservableCollection<SimulationSettings> simSettings = new ObservableCollection<SimulationSettings>();

            simSettings.Add(new SimulationSettings { });

            SimulationSettings = simSettings;
        }
    }
}
