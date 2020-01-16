using System;
using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Model {
    public class RoutePoint : BindableBase {
        private string _description;
        private bool _isSelected;
        private Location _location;
        private string _name;

        public RoutePoint(Location location) {
            _location = location;
            Id = Guid.NewGuid();
        }

        public string Description {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public Guid Id { get; }

        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public Location Location {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public string Name {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public void Update(string name, string description) {
            _name = name;
            _description = description;
        }
    }
}