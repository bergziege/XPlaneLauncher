using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MapControl;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Services;
using XPlaneLauncher.Ui.Modules.AircraftList.Events;
using XPlaneLauncher.Ui.Modules.Logbook.Events;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.Events;
using XPlaneLauncher.Ui.Modules.RouteEditor.Events;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime {
    public class MapViewModel : BindableBase, IMapViewModel, IWeakEventListener {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRouteService _routeService;
        private DelegateCommand<Location> _locationSelectedCommand;
        private DelegateCommand<MapBoundary> _mapBoundariesChangedCommand;
        private Location _mapCenter;
        private ObservableCollection<LocationCollection> _tracks = new ObservableCollection<LocationCollection>();
        private BoundingBox _boundingBox;

        public MapViewModel(
            IAircraftModelProvider modelProvider, IRouteService routeService,
            IEventAggregator eventAggregator) {
            _routeService = routeService;
            _eventAggregator = eventAggregator;
            Aircrafts = modelProvider.Aircrafts;
            eventAggregator.GetEvent<PubSubEvent<AircraftsLoadedEvent>>().Subscribe(OnAircraftsLoaded);
            eventAggregator.GetEvent<PubSubEvent<RoutePointRemovedEvent>>().Subscribe(OnRoutePointRemoved);
            eventAggregator.GetEvent<PubSubEvent<RoutePointAddedEvent>>().Subscribe(OnRoutePointAdded);
            eventAggregator.GetEvent<PubSubEvent<SelectionChangedEvent>>().Subscribe(OnAircraftListSelectioChanged);
            eventAggregator.GetEvent<PubSubEvent<AircraftRemovedEvent>>().Subscribe(OnAircraftRemoved);
            eventAggregator.GetEvent<PubSubEvent<VisualizeTrackEvent>>().Subscribe(OnVisualizeTrack);
        }

        public ObservableCollection<Aircraft> Aircrafts { get; }

        public DelegateCommand<Location> LocationSelectedCommand {
            get { return _locationSelectedCommand ?? (_locationSelectedCommand = new DelegateCommand<Location>(OnLocationSelected)); }
        }

        public DelegateCommand<MapBoundary> MapBoundariesChangedCommand {
            get { return _mapBoundariesChangedCommand ?? (_mapBoundariesChangedCommand = new DelegateCommand<MapBoundary>(OnBoundariesChanged)); }
        }

        public Location MapCenter {
            get { return _mapCenter; }
            set { SetProperty(ref _mapCenter, value); }
        }

        public ObservableCollection<RoutePoint> RoutePoints { get; } = new ObservableCollection<RoutePoint>();

        public ObservableCollection<AircraftRouteViewModel> Routes { get; } = new ObservableCollection<AircraftRouteViewModel>();

        public ObservableCollection<LocationCollection> Tracks {
            get { return _tracks; }
            private set { SetProperty(ref _tracks, value, nameof(Tracks)); }
        }

        public BoundingBox BoundingBox {
            get { return _boundingBox;}
            set {
                SetProperty(ref _boundingBox, value, nameof(BoundingBox));; }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            if (managerType == typeof(PropertyChangedEventManager) && e is PropertyChangedEventArgs args &&
                args.PropertyName == nameof(Aircraft.IsSelected)) {
                HighlightSelectedAircraftRoute();
            }

            return true;
        }

        private void CenterOnSelectedAircaft() {
            Aircraft selected = Aircrafts.FirstOrDefault(x => x.IsSelected);
            if (selected != null && selected.Location != null) {
                MapCenter = new Location(selected.Location.Latitude, selected.Location.Longitude);
            }
        }

        private void HighlightSelectedAircraftRoute() {
            foreach (AircraftRouteViewModel aircraftRouteViewModel in Routes) {
                aircraftRouteViewModel.IsSelected = false;
            }

            Aircraft selectedCraft = Aircrafts.FirstOrDefault(x => x.IsSelected);
            if (selectedCraft != null) {
                AircraftRouteViewModel aircraftRouteViewModel = Routes.FirstOrDefault(x => x.AircraftId == selectedCraft.Id);
                if (aircraftRouteViewModel != null) {
                    aircraftRouteViewModel.IsSelected = true;
                }
            }
        }

        private void OnAircraftListSelectioChanged(SelectionChangedEvent obj) {
            CenterOnSelectedAircaft();
            HighlightSelectedAircraftRoute();
        }

        private void OnAircraftRemoved(AircraftRemovedEvent obj) {
            List<RoutePoint> routePointsToRemove = RoutePoints.Where(x => x.AircraftId == obj.AircraftId).ToList();
            AircraftRouteViewModel routeToRemove = Routes.FirstOrDefault(x => x.AircraftId == obj.AircraftId);

            if (routePointsToRemove.Any()) {
                foreach (RoutePoint routePoint in routePointsToRemove) {
                    RoutePoints.Remove(routePoint);
                }
            }

            if (routeToRemove != null) {
                Routes.Remove(routeToRemove);
            }
        }

        private void OnAircraftsLoaded(AircraftsLoadedEvent obj) {
            RefreshRoutePointsAndRoutes();
            foreach (Aircraft aircraft in Aircrafts) {
                PropertyChangedEventManager.AddListener(aircraft, this, nameof(aircraft.IsSelected));
            }
        }

        private void OnBoundariesChanged(MapBoundary obj) {
            _eventAggregator.GetEvent<PubSubEvent<MapBoundariesChangedEvent>>().Publish(new MapBoundariesChangedEvent(obj));
        }

        private void OnLocationSelected(Location obj) {
            _eventAggregator.GetEvent<PubSubEvent<LocationSelectedEvent>>().Publish(new LocationSelectedEvent(obj));
        }

        private void OnRoutePointAdded(RoutePointAddedEvent obj) {
            RoutePoints.Add(obj.AddedRoutePoint);
            AircraftRouteViewModel route = Routes.FirstOrDefault(x => x.AircraftId == obj.AircraftId);
            if (route != null) {
                Routes.Remove(route);
            }

            Aircraft aircraft = Aircrafts.FirstOrDefault(x => x.Id == obj.AircraftId);
            if (aircraft != null) {
                route = _routeService.GetRouteLine(aircraft);
                route.IsSelected = true;
                Routes.Add(route);
            }
        }

        private void OnRoutePointRemoved(RoutePointRemovedEvent obj) {
            RoutePoint routePoint = RoutePoints.FirstOrDefault(x => x.Id == obj.RoutePointId);
            if (routePoint != null) {
                RoutePoints.Remove(routePoint);
            }

            AircraftRouteViewModel routeViewModel = Routes.FirstOrDefault(x => x.AircraftId == obj.AircraftId);
            if (routeViewModel != null) {
                Routes.Remove(routeViewModel);
            }

            Aircraft aircraft = Aircrafts.FirstOrDefault(x => x.Id == obj.AircraftId);
            if (aircraft != null) {
                AircraftRouteViewModel route = _routeService.GetRouteLine(aircraft);
                route.IsSelected = true;
                Routes.Add(route);
            }
        }

        private void OnVisualizeTrack(VisualizeTrackEvent obj) {
            Tracks.Clear();
            if (obj.Track != null && obj.Track.Any()) {
                if (obj.Track.Count == 2) {
                    Tracks.Add(obj.Track.First().CalculateGreatCircleLocations(obj.Track.Last()));
                }else if (obj.Track.Count > 2) {
                    LocationCollection locations = new LocationCollection();
                    foreach (Location location in obj.Track) {
                        locations.Add(location);
                    }
                    Tracks.Add(locations);
                }
                ZoomToTracksBounds();
            }
        }

        private void ZoomToTracksBounds() {
            double west = Tracks.SelectMany(x => x).Min(x => x.Longitude) -1;
            double east = Tracks.SelectMany(x => x).Max(x => x.Longitude) +1;
            double north = Tracks.SelectMany(x => x).Max(x => x.Latitude) +1;
            double south = Tracks.SelectMany(x => x).Min(x => x.Latitude) -1;
            BoundingBox = new BoundingBox(south, west, north, east);
        }

        private void RefreshRoutePointsAndRoutes() {
            RoutePoints.Clear();
            Routes.Clear();
            foreach (Aircraft aircraft in Aircrafts) {
                RoutePoints.AddRange(aircraft.Route);
                Routes.Add(_routeService.GetRouteLine(aircraft));
            }
        }
    }
}