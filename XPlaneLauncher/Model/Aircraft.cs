using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MapControl;
using Prism.Mvvm;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Model {
    public class Aircraft : BindableBase {
        private readonly AircraftLauncherInformation _launcherInfo;
        private bool _isSelected;
        private bool _isVisible = true;
        private string _livery;
        private Location _location;
        private string _name;
        private double _routeLength;
        private Situation _situation;
        private Thumbnail _thumbnail;

        public Aircraft(AircraftInformation aircraftInformation, AircraftLauncherInformation launcherInfo) {
            Id = Guid.NewGuid();
            _launcherInfo = launcherInfo;
            AircraftInformation = aircraftInformation;
        }

        public AircraftInformation AircraftInformation { get; }

        public Guid Id { get; }

        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public bool IsVisible {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value, nameof(IsVisible)); }
        }

        public FileInfo LauncherInfoFile {
            get { return _launcherInfo?.LauncherInfoFile; }
        }

        public string Livery {
            get { return _livery; }
            private set { SetProperty(ref _livery, value); }
        }

        public Location Location {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public string Name {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        public ObservableCollection<RoutePoint> Route { get; } = new ObservableCollection<RoutePoint>();

        public double RouteLength {
            get { return _routeLength; }
            private set { SetProperty(ref _routeLength, value); }
        }

        public Situation Situation {
            get { return _situation; }
            private set { SetProperty(ref _situation, value); }
        }

        public Thumbnail Thumbnail {
            get { return _thumbnail; }
            private set { SetProperty(ref _thumbnail, value); }
        }

        public void Init() {
            if (_launcherInfo != null) {
                Name = AircraftInformation.AircraftFile.Replace(".acf", "");
                Livery = GetLastParthOfLiveryPath(AircraftInformation.Livery);
                Location = new Location(AircraftInformation.Latitude, AircraftInformation.Longitude);
                foreach (Location location in _launcherInfo.TargetLocation) {
                    LocationInformation additionalInformation = null;
                    if (_launcherInfo.LocationInformations != null && _launcherInfo.LocationInformations.ContainsKey(location)) {
                        additionalInformation = _launcherInfo.LocationInformations[location];
                    }

                    RoutePoint routePoint = new RoutePoint(location, Id);
                    if (additionalInformation != null) {
                        routePoint.Update(additionalInformation.Name, additionalInformation.Comment);
                    }

                    Route.Add(routePoint);
                }
            }
        }

        public void Update(Situation sit) {
            Situation = sit;
        }

        public void Update(Thumbnail thumbnail) {
            Thumbnail = thumbnail;
        }

        public void Update(double routeLength) {
            RouteLength = routeLength;
        }

        private string GetLastParthOfLiveryPath(string livery) {
            if (livery.EndsWith("/")) {
                livery = livery.Substring(0, livery.Length - 1);
            }

            string[] strings = livery.Split('/');
            if (strings.Any()) {
                return strings.Last();
            }

            return string.Empty;
        }
    }
}