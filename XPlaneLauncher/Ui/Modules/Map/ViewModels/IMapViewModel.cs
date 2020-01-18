using System;
using System.Collections.ObjectModel;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels {
    public interface IMapViewModel {
        ObservableCollection<Aircraft> Aircrafts { get; }
        ObservableCollection<RoutePoint> RoutePoints { get; }
        Location MapCenter { get; set; }
        ObservableCollection<AircraftRouteViewModel> Routes { get; }
        DelegateCommand<Location> LocationSelectedCommand { get; }
        DelegateCommand<MapBoundary> MapBoundariesChangedCommand { get; }
    }
}