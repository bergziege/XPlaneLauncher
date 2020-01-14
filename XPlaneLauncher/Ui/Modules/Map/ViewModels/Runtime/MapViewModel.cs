using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows;
using MapControl;
using Prism.Common;
using Prism.Mvvm;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime {
    public class MapViewModel : BindableBase, IMapViewModel, IWeakEventListener {
        private readonly IRouteService _routeService;
        private Location _mapCenter;

        public MapViewModel(IAircraftModelProvider modelProvider, IRouteService routeService) {
            _routeService = routeService;
            Aircrafts = modelProvider.Aircrafts;
            CollectionChangedEventManager.AddListener(Aircrafts, this);
        }

        public ObservableCollection<Aircraft> Aircrafts { get; }

        public ObservableCollection<RoutePoint> RoutePoints { get; } = new ObservableCollection<RoutePoint>();

        public ObservableCollection<Polyline> Routes { get; } = new ObservableCollection<Polyline>();

        public ObservableCollection<Polyline> SelectedRoute { get; } = new ObservableCollection<Polyline>();

        public Location MapCenter {
            get { return _mapCenter; }
            set { SetProperty(ref _mapCenter, value); }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            if (managerType == typeof(CollectionChangedEventManager)) {
                RefreshRoutePointsAndRoutes();
                foreach (Aircraft aircraft in Aircrafts) {
                    PropertyChangedEventManager.AddListener(aircraft, this, nameof(aircraft.IsSelected));
                }
            } else {
                CenterOnSelectedAircaft();
            }
            return true;
        }

        private void CenterOnSelectedAircaft() {
            Aircraft selected = Aircrafts.FirstOrDefault(x => x.IsSelected);
            if (selected != null) {
                MapCenter = new Location(selected.Location.Latitude, selected.Location.Longitude);
            }
        }

        private void UpdateSelectedRoute() {
            SelectedRoute.Clear();
            Aircraft aircraft = Aircrafts.FirstOrDefault(x => x.IsSelected);
            if (aircraft != null) {
                SelectedRoute.Add(_routeService.GetRouteLine(aircraft));
            }
        }

        private void RefreshRoutePointsAndRoutes() {
            RoutePoints.Clear();
            Routes.Clear();
            foreach (Aircraft aircraft in Aircrafts) {
                RoutePoints.AddRange(aircraft.Route);
                Routes.Add(_routeService.GetRouteLine(aircraft));
            }
            foreach (RoutePoint routePoint in Aircrafts.SelectMany(x => x.Route)) {
                RoutePoints.Add(routePoint);
            }
        }
    }
}