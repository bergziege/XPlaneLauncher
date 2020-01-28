using System.Threading.Tasks;
using MapControl;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services {
    public interface IAircraftService {
        Task ReloadAsync();
        void RemoveRoutePointFromAircraft(Aircraft aircraft, RoutePoint routePoint);
        RoutePoint AddRoutePointToAircraft(Aircraft aircraft, Location location);
        void Save(Aircraft aircraft);
        void RemoveAircraft(Aircraft aircraft);
    }
}