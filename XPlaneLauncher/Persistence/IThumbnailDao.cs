using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Persistence {
    public interface IThumbnailDao {
        Thumbnail FindThumbnail(AircraftInformation aircraftInformation);
    }
}