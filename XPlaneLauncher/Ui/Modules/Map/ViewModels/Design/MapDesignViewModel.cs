using System.Collections.ObjectModel;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Design {
    public class MapDesignViewModel : IMapViewModel {
        public ObservableCollection<Aircraft> Aircrafts { get; } = new ObservableCollection<Aircraft>();

        public ObservableCollection<RoutePoint> RoutePoints { get; } = new ObservableCollection<RoutePoint>();
        public Location MapCenter { get; set; }
        public ObservableCollection<AircraftRouteViewModel> Routes { get; }
        public DelegateCommand<Location> LocationSelectedCommand { get; }
        public DelegateCommand<MapBoundary> MapBoundariesChangedCommand { get; }
    }
}