using System.IO;
using System.Threading.Tasks;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence {
    public interface ILauncherInformationDao {
        Task<AircraftLauncherInformation> FindOrCreateForAircraftAsync(AircraftInformation aircraftInformation);
        void SaveToFile(FileInfo launcherInfoFile, AircraftLauncherInformation launcherInfo);
        void Delete(FileInfo aircraftLauncherInfoFile);
    }
}