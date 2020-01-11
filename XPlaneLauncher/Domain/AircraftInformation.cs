using System.IO;
using Newtonsoft.Json;

namespace XPlaneLauncher.Domain {
    public class AircraftInformation {
        public string AircraftFile { get; set; }
        public double Elevation { get; set; }

        [JsonIgnore]
        public FileInfo File { get; set; }

        public double Heading { get; set; }
        public double Latitude { get; set; }
        public string Livery { get; set; }
        public double Longitude { get; set; }
    }
}