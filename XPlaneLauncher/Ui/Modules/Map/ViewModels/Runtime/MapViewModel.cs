using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows;
using Prism.Common;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime {
    public class MapViewModel : IMapViewModel, IWeakEventListener {
        private readonly IRouteService _routeService;

        public MapViewModel(IAircraftModelProvider modelProvider, IRouteService routeService) {
            _routeService = routeService;
            Aircrafts = modelProvider.Aircrafts;
            CollectionChangedEventManager.AddListener(Aircrafts, this);
        }

        public ObservableCollection<Aircraft> Aircrafts { get; }

        public ObservableCollection<RoutePoint> RoutePoints { get; } = new ObservableCollection<RoutePoint>();

        public ObservableCollection<Polyline> Routes { get; } = new ObservableCollection<Polyline>();

        public ObservableCollection<Polyline> SelectedRoute { get; } = new ObservableCollection<Polyline>();

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            RefreshRoutePointsAndRoutes();
            return true;
        }

        private void UpdateSelectedRoute() {
            SelectedRoute.Clear();
            Aircraft firstOrDefault = Aircrafts.FirstOrDefault(x => x.IsSelected);
            if (firstOrDefault != null) {
                SelectedRoute.Add(_routeService.GetRouteLine(firstOrDefault, firstOrDefault.Route));
            }
        }

        private void RefreshRoutePointsAndRoutes() {
            RoutePoints.Clear();
            Routes.Clear();
            foreach (Aircraft aircraft in Aircrafts) {
                RoutePoints.AddRange(aircraft.Route);
                Routes.Add(_routeService.GetRouteLine(aircraft, aircraft.Route));
            }
            foreach (RoutePoint routePoint in Aircrafts.SelectMany(x => x.Route)) {
                RoutePoints.Add(routePoint);
            }
        }
    }
}