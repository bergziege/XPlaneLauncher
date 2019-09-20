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
        private Location _targetLocation;
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

        public Location TargetLocation
        {
            get { return _targetLocation; }
            private set { SetProperty(ref _targetLocation, value); }
        }

        public void UpdateTarget(Location targetLocation)
        {
            TargetLocation = targetLocation;
            PathToTarget.Clear();
            Location origin = new Location(AircraftDto.Lat, AircraftDto.Lon);
            if (targetLocation != null)
            {
                PathToTarget.Add(new Polyline() { Locations = origin.CalculateGreatCircleLocations(targetLocation) }); 
            }
            UpdateDistanceToTarget(origin, targetLocation);
        }

        public DelegateCommand RemoveTargetCommand
        {
            get
            {
                if (_removeTargetCommand == null)
                {
                    _removeTargetCommand = new DelegateCommand(RemoveTarget);
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
                TargetLocation = aircraftLauncherInformation.TargetLocation;
                UpdateTarget(TargetLocation);
            }
            return this;
        }

        private void UpdateDistanceToTarget(Location origin, Location destination)
        {
            if (destination != null)
            {
                DistanceToDestination = origin.GreatCircleDistance(destination) / 1000;
            }
            else
            {
                DistanceToDestination = null;
            }
        }

        public ObservableCollection<Polyline> PathToTarget { get; } = new ObservableCollection<Polyline>();

        public double? DistanceToDestination
        {
            get => _distanceToDestination;
            private set => SetProperty(ref _distanceToDestination, value);
        }

        private void RemoveTarget()
        {
            TargetLocation = null;
            SaveLauncherInfo();
            UpdateTarget(TargetLocation);
        }

        private void EndTargetSelectionMode()
        {
            IsInTargetSelectionMode = false;
            SaveLauncherInfo();
        }

        private void SaveLauncherInfo()
        {
            _aircraftService.SaveLauncherInformation(AircraftDto,
                new AircraftLauncherInformation() {TargetLocation = TargetLocation});
        }
    }
}