using System.Threading.Tasks;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence {
    public interface ILauncherInformationDao {
        Task<AircraftLauncherInformation> FindForAircraftAsync(AircraftInformation aircraftInformation);
    }
}