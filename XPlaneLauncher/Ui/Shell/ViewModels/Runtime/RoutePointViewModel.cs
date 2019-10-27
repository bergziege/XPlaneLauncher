using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Runtime {
    public class RoutePointViewModel : BindableBase, IRoutePointViewModel {
        private bool _isSelected;
        private Location _location;
        private string _name;
        private string _comment;

        public RoutePointViewModel(Location aircraftPlannedRoutePoint) {
            Location = aircraftPlannedRoutePoint;
        }

        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public Location Location {
            get { return _location; }
            private set { SetProperty(ref _location, value); }
        }

        public void Deselect() {
            IsSelected = false;
        }

        public void Select() {
            IsSelected = true;
        }

        public string Name {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Comment {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }
    }
}