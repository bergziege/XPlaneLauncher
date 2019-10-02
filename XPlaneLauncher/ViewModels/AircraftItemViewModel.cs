using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IList<Location> _plannedRoutePoints = new List<Location>();
        private DelegateCommand _removeTargetCommand;
        private double? _distanceToDestination;

        public AircraftItemViewModel(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        public string ThumbnailPath { get; private set; }
        public string Name { get; private set; }
        public string Livery { get; private set; }
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
                    _endTargetSelectioNmodeCommand = new DelegateCommand(EndTargetSelectionMode);
                }

                return _endTargetSelectioNmodeCommand;
            }
        }

        public IList<Location> PlannedRoutePoints
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
                LocationCollection pathLocations = new LocationCollection();
                foreach (Location segmentEnd in PlannedRoutePoints)
                {
                    DistanceToDestination += segmentStart.GreatCircleDistance(segmentEnd);
                    pathLocations.AddRange(segmentStart.CalculateGreatCircleLocations(segmentEnd));
                    segmentStart = segmentEnd;
                }
                PathToTarget.Add(new Polyline() {Locations = pathLocations});
            }
        }

        public DelegateCommand RemoveTargetCommand
        {
            get
            {
                if (_removeTargetCommand == null)
                {
                    _removeTargetCommand = new DelegateCommand(RemoveLastPlannedRoutePoint);
                }

                return _removeTargetCommand;
            }
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
            HasSitFile = AircraftDto.HasSit;

            AircraftLauncherInformation aircraftLauncherInformation = _aircraftService.GetLauncherInformation(AircraftDto);
            if (aircraftLauncherInformation != null)
            {
                PlannedRoutePoints = aircraftLauncherInformation.TargetLocation;
                UpdatePlannedRoutePathAndDistance();
            }
            return this;
        }

        public ObservableCollection<Polyline> PathToTarget { get; } = new ObservableCollection<Polyline>();

        public double? DistanceToDestination
        {
            get => _distanceToDestination;
            private set => SetProperty(ref _distanceToDestination, value);
        }

        private void RemoveLastPlannedRoutePoint()
        {
            if (PlannedRoutePoints.Any()) {
                PlannedRoutePoints.Remove(PlannedRoutePoints.Last()); 
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