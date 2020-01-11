using System;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using MapControl;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Model {
    public class Aircraft : BindableBase {
        private AircraftLauncherInformation _launcherInfo;
        private Situation _situation;
        private Thumbnail _thumbnail;

        public Aircraft(AircraftInformation aircraftInformation, AircraftLauncherInformation launcherInfo) {
            _launcherInfo = launcherInfo;
            AircraftInformation = aircraftInformation;
        }

        public AircraftInformation AircraftInformation { get; }
        public ObservableCollection<RoutePoint> Route { get; } = new ObservableCollection<RoutePoint>();

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
                foreach (Location location in _launcherInfo.TargetLocation) {
                    LocationInformation additionalInformation = null;
                    if (_launcherInfo.LocationInformations != null && _launcherInfo.LocationInformations.ContainsKey(location)) {
                        additionalInformation = _launcherInfo.LocationInformations[location];
                    }

                    RoutePoint routePoint = new RoutePoint(location);
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
    }
}