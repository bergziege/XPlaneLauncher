using System.Collections.ObjectModel;
using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Map
{
    public class AircraftItem : BindableBase
    {
        public AircraftItem(string name, double lat, double lon, IAircraftItemViewModel aircraftViewModel)
        {
            Name = name;
            AircraftViewModel = aircraftViewModel;
            Location = new Location(lat, lon);
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public IAircraftItemViewModel AircraftViewModel { get; }

        private Location _location;
        private bool _isSelected;

        public Location Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }
    }
}