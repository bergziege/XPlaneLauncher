using System.Collections.Generic;
using System.IO;
using MapControl;
using Newtonsoft.Json;

namespace XPlaneLauncher.Domain
{
    public class AircraftLauncherInformation
    {
        public IList<Location> TargetLocation { get; set; }

        public IDictionary<Location, LocationInformation> LocationInformations { get; set; }

        [JsonIgnore]
        public FileInfo LauncherInfoFile { get; set; }

        public double SummaryDistanceNauticalMiles { get; set; }

        public double SummaryHours { get; set; }
    }
}