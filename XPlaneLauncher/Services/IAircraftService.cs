using System.Threading.Tasks;

namespace XPlaneLauncher.Services {
    public interface IAircraftService {
        Task ReloadAsync();
    }
}