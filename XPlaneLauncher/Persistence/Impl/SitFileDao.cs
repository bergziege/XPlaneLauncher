using System.IO;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Extensions;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Persistence.Impl {
    public class SitFileDao : ISitFileDao {
        public Situation FindSit(AircraftInformation aircraftInformation) {
            DirectoryInfo xplaneRootPath = new DirectoryInfo(Properties.Settings.Default.XPlaneRootPath);
            DirectoryInfo sitFilePath = new DirectoryInfo(
                Path.Combine(xplaneRootPath.FullName, Properties.Settings.Default.SituationsPathRelativeToXPlaneRoot));
            FileInfo sitFile = new FileInfo($@"{sitFilePath.FullName}{Path.DirectorySeparatorChar}{aircraftInformation.File.NameWithoutExtension()}.sit");
            return new Situation(sitFile.Exists, sitFile);
        }
    }
}