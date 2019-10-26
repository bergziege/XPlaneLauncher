using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Runtime {
    public class RoutePointViewModel : BindableBase, IRoutePointViewModel {
        private bool _isSelected;
        private Location _location;

        public RoutePointViewModel(Location aircraftPlannedRoutePoint) {
            Location = aircraftPlannedRoutePoint;
        }

        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value, nameof(IsSelected)); }
        }

        public Location Location {
            get { return _location; }
            private set { SetProperty(ref _location, value, nameof(Location)); }
        }

        public void Deselect() {
            IsSelected = false;
        }

        public void Select() {
            IsSelected = true;
        }
    }
}