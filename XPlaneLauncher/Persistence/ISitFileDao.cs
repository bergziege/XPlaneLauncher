using System.IO;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Persistence {
    public interface ISitFileDao {
        Situation FindSit(AircraftInformation aircraftInformation);
        void Delete(FileInfo aircraftSituation);
    }
}