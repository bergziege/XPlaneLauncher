using System.Threading.Tasks;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services {
    public interface IAircraftService {
        Task ReloadAsync();
        void RemoveRoutePoint(Aircraft aircraft, RoutePoint routePoint);
    }
}