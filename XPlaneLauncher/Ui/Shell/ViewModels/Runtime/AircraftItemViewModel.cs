using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MapControl;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Services.Impl;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Runtime {
    public class AircraftItemViewModel : BindableBase, IAircraftItemViewModel, IWeakEventListener {
        //private readonly AircraftService _aircraftService;
        //private double? _distanceToDestination;
        //private DelegateCommand _endTargetSelectioNmodeCommand;
        //private bool _isInTargetSelectionMode;
        //private bool _isSelected;
        //private bool _isVisible = true;
        //private string _lastPartOfLiveryPath;
        //private ObservableCollection<IRoutePointViewModel> _plannedRoutePoints = new ObservableCollection<IRoutePointViewModel>();
        //private DelegateCommand _removeTargetCommand;
        //private IRoutePointViewModel _selectedPlannedRoutePoint;
        //private DelegateCommand _startTargetSelectionModeCommand;

        //public AircraftItemViewModel(AircraftService aircraftService) {
        //    _aircraftService = aircraftService;
        //}

        //public AircraftDto AircraftDto { get; private set; }

        //public double? DistanceToDestination {
        //    get => _distanceToDestination;
        //    private set => SetProperty(ref _distanceToDestination, value);
        //}

        //public DelegateCommand EndTargetSelectionModeCommand {
        //    get {
        //        if (_endTargetSelectioNmodeCommand == null) {
        //            _endTargetSelectioNmodeCommand =
        //                new DelegateCommand(EndTargetSelectionMode);
        //        }

        //        return _endTargetSelectioNmodeCommand;
        //    }
        //}

        //public bool HasSitFile { get; private set; }
        //public bool HasThumbnail { get; private set; }

        //public bool IsInTargetSelectionMode {
        //    get { return _isInTargetSelectionMode; }
        //    private set { SetProperty(ref _isInTargetSelectionMode, value); }
        //}

        //public bool IsSelected {
        //    get { return _isSelected; }
        //    set {
        //        if (IsInTargetSelectionMode) {
        //            return;
        //        }

        //        SetProperty(ref _isSelected, value);
        //    }
        //}

        //public bool IsVisible {
        //    get => _isVisible;
        //    set => SetProperty(ref _isVisible, value, nameof(IsVisible));
        //}

        //public string LastPartOfLiveryPath {
        //    get { return _lastPartOfLiveryPath; }
        //    private set { SetProperty(ref _lastPartOfLiveryPath, value, nameof(LastPartOfLiveryPath)); }
        //}

        //public string Livery { get; private set; }
        //public string Name { get; private set; }

        //public ObservableCollection<Polyline> PathToTarget { get; } = new ObservableCollection<Polyline>();

        //public ObservableCollection<IRoutePointViewModel> PlannedRoutePoints {
        //    get { return _plannedRoutePoints; }
        //    private set { SetProperty(ref _plannedRoutePoints, value); }
        //}

        //public DelegateCommand RemoveSelectedRouteLocationCommand {
        //    get {
        //        if (_removeTargetCommand == null) {
        //            _removeTargetCommand = new DelegateCommand(RemoveSelectedPlannedRoutePoint, CanRemoveSelectedPlannedRoutePoint);
        //        }

        //        return _removeTargetCommand;
        //    }
        //}

        //public IRoutePointViewModel SelectedPlannedRoutePoint {
        //    get { return _selectedPlannedRoutePoint; }
        //    set {
        //        _selectedPlannedRoutePoint?.Deselect();
        //        value?.Select();
        //        SetProperty(ref _selectedPlannedRoutePoint, value, nameof(SelectedPlannedRoutePoint));
        //        RemoveSelectedRouteLocationCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public DelegateCommand StartTargetSelectionModeCommand {
        //    get {
        //        if (_startTargetSelectionModeCommand == null) {
        //            _startTargetSelectionModeCommand = new DelegateCommand(StartTargetSelectionMode);
        //        }

        //        return _startTargetSelectionModeCommand;
        //    }
        //}

        //public string ThumbnailPath { get; private set; }

        //public void AddToPlannedRoute(Location location) {
        //    PlannedRoutePoints.Add(new RoutePointViewModel(location));
        //    UpdatePlannedRoutePathAndDistance();
        //}

        //public IAircraftItemViewModel Initialize(AircraftDto aircraft) {
        //    AircraftDto = aircraft;
        //    if (AircraftDto.Thumbnail != null) {
        //        HasThumbnail = true;
        //        ThumbnailPath = AircraftDto.Thumbnail.FullName;
        //    }

        //    Name = AircraftDto.Name;
        //    Livery = AircraftDto.LiveryName;
        //    LastPartOfLiveryPath = GetLastParthOfLiveryPath();
        //    HasSitFile = AircraftDto.HasSit;

        //    AircraftLauncherInformation aircraftLauncherInformation = _aircraftService.GetLauncherInformation(AircraftDto);
        //    if (aircraftLauncherInformation != null) {
        //        PlannedRoutePoints = new ObservableCollection<IRoutePointViewModel>(
        //            aircraftLauncherInformation.TargetLocation.Select(x => new RoutePointViewModel(x)).ToList());
        //        foreach (IRoutePointViewModel routePointViewModel in PlannedRoutePoints) {
        //            if (aircraftLauncherInformation.LocationInformations != null && aircraftLauncherInformation.LocationInformations.ContainsKey(routePointViewModel.Location)) {
        //                LocationInformation locationInformation = aircraftLauncherInformation.LocationInformations[routePointViewModel.Location];
        //                routePointViewModel.Name = locationInformation.Name;
        //                routePointViewModel.Comment = locationInformation.Comment;
        //            }
        //            PropertyChangedEventManager.AddListener(routePointViewModel, this, nameof(routePointViewModel.IsSelected));
        //            PropertyChangedEventManager.AddListener(routePointViewModel, this, nameof(routePointViewModel.Name));
        //            PropertyChangedEventManager.AddListener(routePointViewModel, this, nameof(routePointViewModel.Comment));
        //        }
        //        UpdatePlannedRoutePathAndDistance();
        //    }

        //    return this;
        //}

        //private void StartTargetSelectionMode() {
        //    IsSelected = true;
        //    IsInTargetSelectionMode = true;
        //}

        //private void UpdatePlannedRoutePathAndDistance() {
        //    PathToTarget.Clear();
        //    DistanceToDestination = null;
        //    Location segmentStart = new Location(AircraftDto.Lat, AircraftDto.Lon);
        //    if (PlannedRoutePoints.Any()) {
        //        double distance = 0;
        //        LocationCollection pathLocations = new LocationCollection();
        //        foreach (Location segmentEnd in PlannedRoutePoints.Select(x => x.Location)) {
        //            distance += segmentStart.GreatCircleDistance(segmentEnd);
        //            pathLocations.AddRange(segmentStart.CalculateGreatCircleLocations(segmentEnd));
        //            segmentStart = segmentEnd;
        //        }

        //        PathToTarget.Add(new Polyline() { Locations = pathLocations });
        //        DistanceToDestination = distance / 1000;
        //    }
        //}

        //private bool CanRemoveSelectedPlannedRoutePoint() {
        //    return SelectedPlannedRoutePoint != null;
        //}

        //private string GetLastParthOfLiveryPath() {
        //    string liveryToSplit = Livery;
        //    if (liveryToSplit.EndsWith("/")) {
        //        liveryToSplit = liveryToSplit.Substring(0, liveryToSplit.Length - 1);
        //    }

        //    string[] strings = liveryToSplit.Split('/');
        //    if (strings.Any()) {
        //        return strings.Last();
        //    }

        //    return string.Empty;
        //}

        //private void RemoveSelectedPlannedRoutePoint() {
        //    if (PlannedRoutePoints.Any() && SelectedPlannedRoutePoint != null) {
        //        PlannedRoutePoints.Remove(SelectedPlannedRoutePoint);
        //    }

        //    SaveLauncherInfo();
        //    UpdatePlannedRoutePathAndDistance();
        //}

        //private void EndTargetSelectionMode() {
        //    IsInTargetSelectionMode = false;
        //    SaveLauncherInfo();
        //}

        //private void SaveLauncherInfo() {
        //    IDictionary<Location, LocationInformation> locationInformations = new Dictionary<Location, LocationInformation>();
        //    foreach (IRoutePointViewModel routePointViewModel in PlannedRoutePoints) {
        //        locationInformations.Add(routePointViewModel.Location, new LocationInformation(){Name = routePointViewModel.Name, Comment = routePointViewModel.Comment});
        //    }
        //    _aircraftService.SaveLauncherInformation(
        //        AircraftDto,
        //        new AircraftLauncherInformation() {
        //            TargetLocation = locationInformations.Keys.ToList(),
        //            LocationInformations = locationInformations
        //        });
        //}

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            //if (sender is IRoutePointViewModel routePointViewModel && e is PropertyChangedEventArgs args) {
            //    switch (args.PropertyName) {
            //        case nameof(routePointViewModel.IsSelected):
            //            SelectedPlannedRoutePoint = PlannedRoutePoints.FirstOrDefault(x => x.IsSelected);
            //            break;
            //        case nameof(routePointViewModel.Name):
            //        case nameof(routePointViewModel.Comment):
            //            SaveLauncherInfo();
            //            break;
            //    }
            //}
            return true;
        }
    }
}