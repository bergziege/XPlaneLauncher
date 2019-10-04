using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MapControl;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.ViewModels
{
    public class AircraftItemViewModel : BindableBase, IAircraftItemViewModel
    {
        private readonly AircraftService _aircraftService;
        private bool _isSelected;
        private bool _isInTargetSelectionMode;
        private DelegateCommand _startTargetSelectionModeCommand;
        private DelegateCommand _endTargetSelectioNmodeCommand;
        private ObservableCollection<Location> _plannedRoutePoints = new ObservableCollection<Location>();
        private DelegateCommand _removeTargetCommand;
        private double? _distanceToDestination;
        private Location _selectedPlannedRoutePoint;
        private string _lastPartOfLiveryPath;

        public AircraftItemViewModel(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        public string ThumbnailPath { get; private set; }
        public string Name { get; private set; }
        public string Livery { get; private set; }

        public string LastPartOfLiveryPath
        {
            get { return _lastPartOfLiveryPath; }
            private set { SetProperty(ref _lastPartOfLiveryPath, value, nameof(LastPartOfLiveryPath)); }
        }

        public bool HasSitFile { get; private set; }
        public bool HasThumbnail { get; private set; }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (IsInTargetSelectionMode)
                {
                    return;
                }
                SetProperty(ref _isSelected, value);
            } 
        }

        public AircraftDto AircraftDto { get; private set; }

        public bool IsInTargetSelectionMode
        {
            get { return _isInTargetSelectionMode; }
            private set { SetProperty(ref _isInTargetSelectionMode, value); }
        }

        public DelegateCommand StartTargetSelectionModeCommand
        {
            get
            {
                if (_startTargetSelectionModeCommand == null)
                {
                    _startTargetSelectionModeCommand = new DelegateCommand(StartTargetSelectionMode);
                }

                return _startTargetSelectionModeCommand;
            }
        }

        private void StartTargetSelectionMode()
        {
            IsSelected = true;
            IsInTargetSelectionMode = true;
        }

        public DelegateCommand EndTargetSelectionModeCommand
        {
            get
            {
                if (_endTargetSelectioNmodeCommand == null)
                {
                    _endTargetSelectioNmodeCommand =
                        new DelegateCommand(EndTargetSelectionMode);
                }

                return _endTargetSelectioNmodeCommand;
            }
        }

        public ObservableCollection<Location> PlannedRoutePoints
        {
            get { return _plannedRoutePoints; }
            private set { SetProperty(ref _plannedRoutePoints, value); }
        }

        public void AddToPlannedRoute(Location location)
        {
            PlannedRoutePoints.Add(location);
            UpdatePlannedRoutePathAndDistance();
        }

        private void UpdatePlannedRoutePathAndDistance()
        {
            PathToTarget.Clear();
            DistanceToDestination = null;
            Location segmentStart = new Location(AircraftDto.Lat, AircraftDto.Lon);
            if (PlannedRoutePoints.Any())
            {
                double distance = 0;
                LocationCollection pathLocations = new LocationCollection();
                foreach (Location segmentEnd in PlannedRoutePoints)
                {
                    distance += segmentStart.GreatCircleDistance(segmentEnd);
                    pathLocations.AddRange(segmentStart.CalculateGreatCircleLocations(segmentEnd));
                    segmentStart = segmentEnd;
                }
                PathToTarget.Add(new Polyline() {Locations = pathLocations});
                DistanceToDestination = distance / 1000;
            }
        }

        public DelegateCommand RemoveSelectedRouteLocationCommand
        {
            get
            {
                if (_removeTargetCommand == null)
                {
                    _removeTargetCommand = new DelegateCommand(RemoveSelectedPlannedRoutePoint, CanRemoveSelectedPlannedRoutePoint);
                }

                return _removeTargetCommand;
            }
        }

        private bool CanRemoveSelectedPlannedRoutePoint()
        {
            return SelectedPlannedRoutePoint != null;
        }

        public IAircraftItemViewModel Initialize(AircraftDto aircraft)
        {
            AircraftDto = aircraft;
            if (AircraftDto.Thumbnail != null)
            {
                HasThumbnail = true;
                ThumbnailPath = AircraftDto.Thumbnail.FullName;
            }

            Name = AircraftDto.Name;
            Livery = AircraftDto.LiveryName;
            LastPartOfLiveryPath = GetLastParthOfLiveryPath();
            HasSitFile = AircraftDto.HasSit;

            AircraftLauncherInformation aircraftLauncherInformation = _aircraftService.GetLauncherInformation(AircraftDto);
            if (aircraftLauncherInformation != null)
            {
                PlannedRoutePoints = new ObservableCollection<Location>(aircraftLauncherInformation.TargetLocation);
                UpdatePlannedRoutePathAndDistance();
            }
            return this;
        }

        private string GetLastParthOfLiveryPath ()
        {
            string liveryToSplit = Livery;
            if (liveryToSplit.EndsWith("/"))
            {
                liveryToSplit = liveryToSplit.Substring(0, liveryToSplit.Length - 1);
            }
            string[] strings = liveryToSplit.Split('/');
            if (strings.Any())
            { 
                return strings.Last(); 
            }

            return string.Empty;
        }

        public ObservableCollection<Polyline> PathToTarget { get; } = new ObservableCollection<Polyline>();

        public double? DistanceToDestination
        {
            get => _distanceToDestination;
            private set => SetProperty(ref _distanceToDestination, value);
        }

        public Location SelectedPlannedRoutePoint
        {
            get { return _selectedPlannedRoutePoint; }
            set
            {
                SetProperty(ref _selectedPlannedRoutePoint, value, nameof(SelectedPlannedRoutePoint));
                RemoveSelectedRouteLocationCommand.RaiseCanExecuteChanged();
            }
        }

        private void RemoveSelectedPlannedRoutePoint()
        {
            if (PlannedRoutePoints.Any() && SelectedPlannedRoutePoint != null) {
                PlannedRoutePoints.Remove(SelectedPlannedRoutePoint); 
            }
            SaveLauncherInfo();
            UpdatePlannedRoutePathAndDistance();
        }

        private void EndTargetSelectionMode()
        {
            IsInTargetSelectionMode = false;
            SaveLauncherInfo();
        }

        private void SaveLauncherInfo()
        {
            _aircraftService.SaveLauncherInformation(AircraftDto,
                new AircraftLauncherInformation() {TargetLocation = PlannedRoutePoints});
        }
    }
}