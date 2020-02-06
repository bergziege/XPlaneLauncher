using System.Collections.ObjectModel;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Design {
    public class MapDesignViewModel : IMapViewModel {
        public ObservableCollection<Aircraft> Aircrafts { get; } = new ObservableCollection<Aircraft>();
        public DelegateCommand<Location> LocationSelectedCommand { get; } = new DelegateCommand<Location>(location => { });
        public DelegateCommand<MapBoundary> MapBoundariesChangedCommand { get; } = new DelegateCommand<MapBoundary>(boundary => { });
        public Location MapCenter { get; set; }

        public ObservableCollection<RoutePoint> RoutePoints { get; } = new ObservableCollection<RoutePoint>();
        public ObservableCollection<AircraftRouteViewModel> Routes { get; } = new ObservableCollection<AircraftRouteViewModel>();

        public ObservableCollection<LocationCollection> Tracks { get; } = new ObservableCollection<LocationCollection>();
    }
}