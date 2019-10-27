﻿using System.Collections.Generic;
using MapControl;

namespace XPlaneLauncher.Domain
{
    public class AircraftLauncherInformation
    {
        public IList<Location> TargetLocation { get; set; }

        public IDictionary<Location, LocationInformation> LocationInformations { get; set; }
    }
}