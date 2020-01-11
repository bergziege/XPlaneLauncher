using System.IO;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Persistence.Impl {
    public class ThumbnailDao : IThumbnailDao {
        public Thumbnail FindThumbnail(AircraftInformation aircraftInformation) {
            DirectoryInfo xplaneRootPath = new DirectoryInfo(Properties.Settings.Default.XPlaneRootPath);
            FileInfo thumbnail = new FileInfo(
                $@"{xplaneRootPath.FullName}{Path.DirectorySeparatorChar}{aircraftInformation.Livery}{aircraftInformation.AircraftFile?.Replace(".acf", "")}{Properties.Settings.Default.ThumbnailEnding}");
            if (thumbnail.Exists) {
                return new Thumbnail(thumbnail);
            }

            return null;
        }
    }
}