using System.Collections.ObjectModel;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels {
    public interface IMapViewModel {
        ObservableCollection<Aircraft> Aircrafts { get; }
        DelegateCommand<Location> LocationSelectedCommand { get; }
        DelegateCommand<MapBoundary> MapBoundariesChangedCommand { get; }
        Location MapCenter { get; set; }
        ObservableCollection<RoutePoint> RoutePoints { get; }
        ObservableCollection<AircraftRouteViewModel> Routes { get; }
        ObservableCollection<LocationCollection> Tracks { get; }
    }
}