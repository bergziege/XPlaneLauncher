using MapControl;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Shell.ViewModels;

namespace XPlaneLauncher.Map {
    public class AircraftItem : BindableBase {
        private Location _location;

        private string _name;

        public AircraftItem(string name, double lat, double lon, IAircraftItemViewModel aircraftViewModel) {
            Name = name;
            AircraftViewModel = aircraftViewModel;
            Location = new Location(lat, lon);
        }

        public string Name {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public IAircraftItemViewModel AircraftViewModel { get; }

        public Location Location {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }
    }
}