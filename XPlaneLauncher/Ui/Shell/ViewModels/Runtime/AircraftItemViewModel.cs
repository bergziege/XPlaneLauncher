﻿using System.Collections.ObjectModel;
using System.Linq;
using MapControl;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Runtime {
    public class AircraftItemViewModel : BindableBase, IAircraftItemViewModel {
        private readonly AircraftService _aircraftService;
        private double? _distanceToDestination;
        private DelegateCommand _endTargetSelectioNmodeCommand;
        private bool _isInTargetSelectionMode;
        private bool _isSelected;
        private bool _isVisible = true;
        private string _lastPartOfLiveryPath;
        private ObservableCollection<IRoutePointViewModel> _plannedRoutePoints = new ObservableCollection<IRoutePointViewModel>();
        private DelegateCommand _removeTargetCommand;
        private IRoutePointViewModel _selectedPlannedRoutePoint;
        private DelegateCommand _startTargetSelectionModeCommand;

        public AircraftItemViewModel(AircraftService aircraftService) {
            _aircraftService = aircraftService;
        }

        public string ThumbnailPath { get; private set; }
        public string Name { get; private set; }
        public string Livery { get; private set; }

        public string LastPartOfLiveryPath {
            get { return _lastPartOfLiveryPath; }
            private set { SetProperty(ref _lastPartOfLiveryPath, value, nameof(LastPartOfLiveryPath)); }
        }

        public bool HasSitFile { get; private set; }
        public bool HasThumbnail { get; private set; }

        public bool IsSelected {
            get { return _isSelected; }
            set {
                if (IsInTargetSelectionMode) {
                    return;
                }

                SetProperty(ref _isSelected, value);
            }
        }

        public AircraftDto AircraftDto { get; private set; }

        public bool IsInTargetSelectionMode {
            get { return _isInTargetSelectionMode; }
            private set { SetProperty(ref _isInTargetSelectionMode, value); }
        }

        public DelegateCommand StartTargetSelectionModeCommand {
            get {
                if (_startTargetSelectionModeCommand == null) {
                    _startTargetSelectionModeCommand = new DelegateCommand(StartTargetSelectionMode);
                }

                return _startTargetSelectionModeCommand;
            }
        }

        public DelegateCommand EndTargetSelectionModeCommand {
            get {
                if (_endTargetSelectioNmodeCommand == null) {
                    _endTargetSelectioNmodeCommand =
                        new DelegateCommand(EndTargetSelectionMode);
                }

                return _endTargetSelectioNmodeCommand;
            }
        }

        public ObservableCollection<IRoutePointViewModel> PlannedRoutePoints {
            get { return _plannedRoutePoints; }
            private set { SetProperty(ref _plannedRoutePoints, value); }
        }

        public void AddToPlannedRoute(Location location) {
            PlannedRoutePoints.Add(new RoutePointViewModel(location));
            UpdatePlannedRoutePathAndDistance();
        }

        public DelegateCommand RemoveSelectedRouteLocationCommand {
            get {
                if (_removeTargetCommand == null) {
                    _removeTargetCommand = new DelegateCommand(RemoveSelectedPlannedRoutePoint, CanRemoveSelectedPlannedRoutePoint);
                }

                return _removeTargetCommand;
            }
        }

        public IAircraftItemViewModel Initialize(AircraftDto aircraft) {
            AircraftDto = aircraft;
            if (AircraftDto.Thumbnail != null) {
                HasThumbnail = true;
                ThumbnailPath = AircraftDto.Thumbnail.FullName;
            }

            Name = AircraftDto.Name;
            Livery = AircraftDto.LiveryName;
            LastPartOfLiveryPath = GetLastParthOfLiveryPath();
            HasSitFile = AircraftDto.HasSit;

            AircraftLauncherInformation aircraftLauncherInformation = _aircraftService.GetLauncherInformation(AircraftDto);
            if (aircraftLauncherInformation != null) {
                PlannedRoutePoints = new ObservableCollection<IRoutePointViewModel>(aircraftLauncherInformation.TargetLocation.Select(x=>new RoutePointViewModel(x)).ToList());
                UpdatePlannedRoutePathAndDistance();
            }

            return this;
        }

        public ObservableCollection<Polyline> PathToTarget { get; } = new ObservableCollection<Polyline>();

        public double? DistanceToDestination {
            get => _distanceToDestination;
            private set => SetProperty(ref _distanceToDestination, value);
        }

        public IRoutePointViewModel SelectedPlannedRoutePoint {
            get { return _selectedPlannedRoutePoint; }
            set {
                _selectedPlannedRoutePoint?.Deselect();
                value?.Select();
                SetProperty(ref _selectedPlannedRoutePoint, value, nameof(SelectedPlannedRoutePoint));
                RemoveSelectedRouteLocationCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsVisible {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value, nameof(IsVisible));
        }

        private void StartTargetSelectionMode() {
            IsSelected = true;
            IsInTargetSelectionMode = true;
        }

        private void UpdatePlannedRoutePathAndDistance() {
            PathToTarget.Clear();
            DistanceToDestination = null;
            Location segmentStart = new Location(AircraftDto.Lat, AircraftDto.Lon);
            if (PlannedRoutePoints.Any()) {
                double distance = 0;
                LocationCollection pathLocations = new LocationCollection();
                foreach (Location segmentEnd in PlannedRoutePoints.Select(x=>x.Location)) {
                    distance += segmentStart.GreatCircleDistance(segmentEnd);
                    pathLocations.AddRange(segmentStart.CalculateGreatCircleLocations(segmentEnd));
                    segmentStart = segmentEnd;
                }

                PathToTarget.Add(new Polyline() { Locations = pathLocations });
                DistanceToDestination = distance / 1000;
            }
        }

        private bool CanRemoveSelectedPlannedRoutePoint() {
            return SelectedPlannedRoutePoint != null;
        }

        private string GetLastParthOfLiveryPath() {
            string liveryToSplit = Livery;
            if (liveryToSplit.EndsWith("/")) {
                liveryToSplit = liveryToSplit.Substring(0, liveryToSplit.Length - 1);
            }

            string[] strings = liveryToSplit.Split('/');
            if (strings.Any()) {
                return strings.Last();
            }

            return string.Empty;
        }

        private void RemoveSelectedPlannedRoutePoint() {
            if (PlannedRoutePoints.Any() && SelectedPlannedRoutePoint != null) {
                PlannedRoutePoints.Remove(SelectedPlannedRoutePoint);
            }

            SaveLauncherInfo();
            UpdatePlannedRoutePathAndDistance();
        }

        private void EndTargetSelectionMode() {
            IsInTargetSelectionMode = false;
            SaveLauncherInfo();
        }

        private void SaveLauncherInfo() {
            _aircraftService.SaveLauncherInformation(
                AircraftDto,
                new AircraftLauncherInformation() { TargetLocation = PlannedRoutePoints.Select(x=>x.Location).ToList() });
        }
    }
}