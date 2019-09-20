using System.IO;
using System.Security.RightsManagement;

namespace XPlaneLauncher.Dtos
{
    public class AircraftDto
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string LiveryName { get; set; }
        public FileInfo Thumbnail { get; set; }
        public bool HasSit { get; set; }
        public FileInfo SitFile { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Heading { get; set; }
    }
}