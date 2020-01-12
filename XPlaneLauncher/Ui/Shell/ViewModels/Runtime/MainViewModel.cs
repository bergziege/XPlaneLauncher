using System;
using System.Windows;
using Prism.Mvvm;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Runtime {
    public class MainViewModel : BindableBase, IWeakEventListener {
        //private AircraftService _aircraftService;
        //private DelegateCommand<Location> _applyTargetCommand;
        //private bool _changingSelectedAicraftFromList;
        //private bool _changingSelectedAicraftFromMap;
        //private bool _isListFilteredByMapBoundary;
        //private bool _isMapInSelectionMode;
        //private MapBoundary _lastMapBoundary;
        //private DelegateCommand<MapBoundary> _mapBoundariesChangedCommand;
        //private Location _mapCenter;
        //private DelegateCommand _refreshCommand;
        //private IAircraftItemViewModel _selectedAircraft;
        //private DelegateCommand _showSettingsCommand;
        //private DelegateCommand _startSimCommand;
        //private DelegateCommand _unselectAircraftCommand;

        //public MainViewModel() {
        //    _aircraftService = new AircraftService();
        //    CheckSettings();
        //}

        //public ObservableCollection<IAircraftItemViewModel> Aircrafts { get; } =
        //    new ObservableCollection<IAircraftItemViewModel>();

        //public DelegateCommand<Location> ApplyTargetCommand {
        //    get {
        //        if (_applyTargetCommand == null) {
        //            _applyTargetCommand =
        //                new DelegateCommand<Location>(ApplyMapCenterAsTargetOnAircraftInSelectionMode);
        //        }

        //        return _applyTargetCommand;
        //    }
        //}

        //public bool IsListFilteredByMapBoundary {
        //    get { return _isListFilteredByMapBoundary; }
        //    set {
        //        SetProperty(ref _isListFilteredByMapBoundary, value, nameof(IsListFilteredByMapBoundary));
        //        if (value) {
        //            FilterListByLastBoundary();
        //        } else {
        //            ShowAllAircraft();
        //        }
        //    }
        //}

        //public bool IsMapInSelectionMode {
        //    get { return _isMapInSelectionMode; }
        //    private set { SetProperty(ref _isMapInSelectionMode, value); }
        //}

        //public ObservableCollection<AircraftItem> MapAircraftItems { get; } = new ObservableCollection<AircraftItem>();

        //public DelegateCommand<MapBoundary> MapBoundariesChangedCommand {
        //    get {
        //        if (_mapBoundariesChangedCommand == null) {
        //            _mapBoundariesChangedCommand = new DelegateCommand<MapBoundary>(OnMapBoundariesChanged);
        //        }

        //        return _mapBoundariesChangedCommand;
        //    }
        //}

        //public Location MapCenter {
        //    get => _mapCenter;
        //    set { SetProperty(ref _mapCenter, value); }
        //}

        //public ObservableCollection<IRoutePointViewModel> PathsPoints { get; } = new ObservableCollection<IRoutePointViewModel>();

        //public ObservableCollection<Polyline> PathsToTarget { get; } = new ObservableCollection<Polyline>();

        //public DelegateCommand RefreshCommand {
        //    get {
        //        if (_refreshCommand == null) {
        //            _refreshCommand = new DelegateCommand(OnRefreshRequested);
        //        }

        //        return _refreshCommand;
        //    }
        //}

        //public IAircraftItemViewModel SelectedAircraft {
        //    get => _selectedAircraft;
        //    set {
        //        if (_selectedAircraft != value && _selectedAircraft != null && _selectedAircraft.EndTargetSelectionModeCommand.CanExecute()) {
        //            _selectedAircraft.EndTargetSelectionModeCommand.Execute();
        //        }

        //        _changingSelectedAicraftFromList = true;
        //        if (_selectedAircraft != null) {
        //            _selectedAircraft.IsSelected = false;
        //        }

        //        SetProperty(ref _selectedAircraft, value);
        //        StartSimCommand.RaiseCanExecuteChanged();
        //        if (value != null) {
        //            value.IsSelected = true;
        //        }

        //        if (!_changingSelectedAicraftFromMap && SelectedAircraft != null) {
        //            MapCenter = new Location(SelectedAircraft.AircraftDto.Lat, SelectedAircraft.AircraftDto.Lon);
        //        }

        //        _changingSelectedAicraftFromList = false;
        //    }
        //}

        //public DelegateCommand ShowSettingsCommand {
        //    get {
        //        if (_showSettingsCommand == null) {
        //            _showSettingsCommand = new DelegateCommand(OnShowSetting);
        //        }

        //        return _showSettingsCommand;
        //    }
        //}

        //private void CheckSettings() {
        //    if (!Directory.Exists(Properties.Settings.Default.XPlaneRootPath) || !Directory.Exists(Properties.Settings.Default.DataPath)
        //                                                                      || !File.Exists(
        //                                                                          Path.Combine(
        //                                                                              Properties.Settings.Default.XPlaneRootPath,
        //                                                                              Properties.Settings.Default.LuaPathRelativeToXPlaneRoot,
        //                                                                              Properties.Settings.Default.LuaScriptFileName))) {
        //        OnShowSetting();
        //    }
        //}

        //private void ShowAllAircraft() {
        //    foreach (IAircraftItemViewModel aircraftItemViewModel in Aircrafts) {
        //        aircraftItemViewModel.IsVisible = true;
        //    }
        //}

        //private void FilterListByLastBoundary() {
        //    Parallel.ForEach(
        //        Aircrafts,
        //        aircraft => {
        //            if (_lastMapBoundary != null && aircraft.AircraftDto.Lat <= _lastMapBoundary.TopLeft.Latitude &&
        //                aircraft.AircraftDto.Lat >= _lastMapBoundary.BottomRight.Latitude
        //                &&
        //                aircraft.AircraftDto.Lon >= _lastMapBoundary.TopLeft.Longitude &&
        //                aircraft.AircraftDto.Lon <= _lastMapBoundary.BottomRight.Longitude) {
        //                aircraft.IsVisible = true;
        //            } else {
        //                aircraft.IsVisible = false;
        //            }
        //        });
        //}

        //private void OnMapBoundariesChanged(MapBoundary mapBoundary) {
        //    _lastMapBoundary = mapBoundary;
        //    if (IsListFilteredByMapBoundary) {
        //        FilterListByLastBoundary();
        //    }
        //}

        //private void OnShowSetting() {
        //    new SettingsViewCommand().Execute();
        //}

        //private void ApplyMapCenterAsTargetOnAircraftInSelectionMode(Location target) {
        //    foreach (IAircraftItemViewModel aircraftItemViewModel in Aircrafts.Where(x => x.IsInTargetSelectionMode)) {
        //        aircraftItemViewModel.AddToPlannedRoute(target);
        //    }
        //}

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            //if (sender is IAircraftItemViewModel aircraft && e is PropertyChangedEventArgs args &&
            //    !_changingSelectedAicraftFromList) {
            //    if (args.PropertyName == nameof(aircraft.IsSelected)) {
            //        _changingSelectedAicraftFromMap = true;
            //        SelectedAircraft = aircraft;
            //        _changingSelectedAicraftFromMap = false;
            //    } else if (args.PropertyName == nameof(aircraft.IsInTargetSelectionMode)) {
            //        if (aircraft.IsInTargetSelectionMode) {
            //            IsMapInSelectionMode = true;
            //        } else {
            //            IsMapInSelectionMode = false;
            //        }
            //    } else if (args.PropertyName == nameof(aircraft.PathToTarget)) {
            //        UpdateTargetPaths();
            //    } else if (args.PropertyName == nameof(aircraft.PlannedRoutePoints)) {
            //        UpdatePathsPoints();
            //    }
            //}

            return true;
        }

        //private void UpdatePathsPoints() {
        //    PathsPoints.Clear();
        //    foreach (IAircraftItemViewModel aircraft in Aircrafts) {
        //        foreach (IRoutePointViewModel aircraftPlannedRoutePoint in aircraft.PlannedRoutePoints) {
        //            PathsPoints.Add(aircraftPlannedRoutePoint);
        //        }
        //    }
        //}

        //private void UpdateTargetPaths() {
        //    PathsToTarget.Clear();
        //    foreach (IAircraftItemViewModel aircraftItemViewModel in Aircrafts.Where(x => x.PathToTarget.Any())) {
        //        PathsToTarget.Add(aircraftItemViewModel.PathToTarget.First());
        //    }
        //}
    }
}