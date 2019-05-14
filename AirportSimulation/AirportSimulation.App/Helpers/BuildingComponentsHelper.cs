namespace AirportSimulation.App.Helpers
{
    using AirportSimulation.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Imaging;

    internal class BuildingComponentsHelper
    {
        private readonly Dictionary<BuildingComponentType, string> _buildingComponentImages = new Dictionary<BuildingComponentType, string>
        {
            { BuildingComponentType.CheckIn, "Resources/check-in.png" },
            { BuildingComponentType.Conveyor, "Resources/conveyor.png" },
            { BuildingComponentType.PA, "Resources/PickUpBaggage.png" },
            { BuildingComponentType.PSC, "Resources/PSCbaggage.png" },
            { BuildingComponentType.ASC, "Resources/AdvancedCheckBaggage.png" },
            { BuildingComponentType.AA, "Resources/airplane-shape.png" },
            { BuildingComponentType.MPA, "Resources/MPApng.png" },
			{ BuildingComponentType.ManyToOneConveyor, "Resources/manytomanyConv.png" }
        };

        public BitmapImage GetBuildingComponentImage (BuildingComponentType type) => 
            GetComponentImage(_buildingComponentImages.FirstOrDefault(x => x.Key == type).Value);

        private BitmapImage GetComponentImage(string fileLocation)
            => new BitmapImage(new Uri($"../../{fileLocation}", UriKind.Relative));
    }
}
