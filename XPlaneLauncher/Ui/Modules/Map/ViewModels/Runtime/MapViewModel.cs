﻿using System;
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