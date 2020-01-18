using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime;

namespace XPlaneLauncher.Services {
    public interface IRouteService {
        double GetRouteLenght(Aircraft aircraft);
        AircraftRouteViewModel GetRouteLine(Aircraft aircraft);
    }
}