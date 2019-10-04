using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using MapControl;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Map;
using XPlaneLauncher.Properties;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.ViewModels
{
    public class MainViewModel : BindableBase, IMainViewModel, IWeakEventListener
    {
        private DelegateCommand _refreshCommand;
        private DelegateCommand _startSimCommand;
        private AircraftService _aircraftService;
        private IAircraftItemViewModel _selectedAircraft = null;
        private Location _mapCenter;
        private bool _changingSelectedAicraftFromMap;
        private bool _changingSelectedAicraftFromList;
        private bool _isMapInSelectionMode;
        private DelegateCommand<Location> _applyTargetCommand;

        public MainViewModel()
        {
            _aircraftService = new AircraftService();
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(OnRefreshRequested);
                }

                return _refreshCommand;
            }
        }

        private void OnRefreshRequested()
        {
            IList<AircraftDto> aircraftDtos = _aircraftService.GetAircrafts();
            Aircrafts.Clear();
            MapAircraftItems.Clear();
            foreach (AircraftDto aircraftDto in aircraftDtos)
            {
                IAircraftItemViewModel itemViewModel =
                    new AircraftItemViewModel(_aircraftService).Initialize(aircraftDto);
                PropertyChangedEventManager.AddListener(itemViewModel, this, nameof(itemViewModel.IsSelected));
                PropertyChangedEventManager.AddListener(itemViewModel, this,
                    nameof(itemViewModel.IsInTargetSelectionMode));
                PropertyChangedEventManager.AddListener(itemViewModel, this, nameof(itemViewModel.PlannedRoutePoints));
                Aircrafts.Add(itemViewModel);
                MapAircraftItems.Add(new AircraftItem(aircraftDto.LiveryName, aircraftDto.Lat, aircraftDto.Lon,
                    itemViewModel));
            }

            UpdateTargetPaths();
            UpdatePathsPoints();
        }

        public DelegateCommand StartSimCommand
        {
            get
            {
                if (_startSimCommand == null)
                {
                    _startSimCommand = new DelegateCommand(OnStartSimRequested, CanStartSim);
                }

                return _startSimCommand;
            }
        }

        private bool CanStartSim()
        {
            return SelectedAircraft != null && SelectedAircraft.HasSitFile;
        }

        private void OnStartSimRequested()
        {
            string xplaneExecutable =
                Path.Combine(Settings.Default.XPlaneRootPath, Settings.Default.XPlaneExecutableFile);
            SelectedAircraft.AircraftDto.SitFile.CopyTo(Path.Combine(Settings.Default.XPlaneRootPath,
                @"Output/Situations/default.sit"), true);
            FileInfo executable = new FileInfo(xplaneExecutable);
            if (executable.Exists)
            {
                Process.Start(xplaneExecutable);
            }
        }

        public ObservableCollection<IAircraftItemViewModel> Aircrafts { get; } =
            new ObservableCollection<IAircraftItemViewModel>();

        public IAircraftItemViewModel SelectedAircraft
        {
            get => _selectedAircraft;
            set
            {
                if (_selectedAircraft != value && _selectedAircraft != null && _selectedAircraft.EndTargetSelectionModeCommand.CanExecute())
                {
                    _selectedAircraft.EndTargetSelectionModeCommand.Execute();
                }

                _changingSelectedAicraftFromList = true;
                if (_selectedAircraft != null)
                {
                    _selectedAircraft.IsSelected = false;
                }

                SetProperty(ref _selectedAircraft, value);
                StartSimCommand.RaiseCanExecuteChanged();
                if (value != null)
                {
                    value.IsSelected = true;
                }

                if (!_changingSelectedAicraftFromMap && SelectedAircraft != null)
                {
                    MapCenter = new Location(SelectedAircraft.AircraftDto.Lat, SelectedAircraft.AircraftDto.Lon);
                }

                _changingSelectedAicraftFromList = false;
            }
        }

        public ObservableCollection<AircraftItem> MapAircraftItems { get; } = new ObservableCollection<AircraftItem>();

        public Location MapCenter
        {
            get => _mapCenter;
            set { SetProperty(ref _mapCenter, value); }
        }

        public bool IsMapInSelectionMode
        {
            get { return _isMapInSelectionMode; }
            private set { SetProperty(ref _isMapInSelectionMode, value); }
        }

        public DelegateCommand<Location> ApplyTargetCommand
        {
            get
            {
                if (_applyTargetCommand == null)
                {
                    _applyTargetCommand =
                        new DelegateCommand<Location>(ApplyMapCenterAsTargetOnAircraftInSelectionMode);
                }

                return _applyTargetCommand;
            }
        }

        public ObservableCollection<Polyline> PathsToTarget { get; } = new ObservableCollection<Polyline>();
        public ObservableCollection<Location> PathsPoints { get; } = new ObservableCollection<Location>();

        private void ApplyMapCenterAsTargetOnAircraftInSelectionMode(Location target)
        {
            foreach (IAircraftItemViewModel aircraftItemViewModel in Aircrafts.Where(x => x.IsInTargetSelectionMode))
            {
                aircraftItemViewModel.AddToPlannedRoute(target);
            }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (sender is IAircraftItemViewModel aircraft && e is PropertyChangedEventArgs args &&
                !_changingSelectedAicraftFromList)
            {
                if (args.PropertyName == nameof(aircraft.IsSelected))
                {
                    _changingSelectedAicraftFromMap = true;
                    SelectedAircraft = aircraft;
                    _changingSelectedAicraftFromMap = false;
                }
                else if (args.PropertyName == nameof(aircraft.IsInTargetSelectionMode))
                {
                    if (aircraft.IsInTargetSelectionMode)
                    {
                        IsMapInSelectionMode = true;
                    }
                    else
                    {
                        IsMapInSelectionMode = false;
                    }
                }
                else if (args.PropertyName == nameof(aircraft.PathToTarget))
                {
                    UpdateTargetPaths();
                }
                else if (args.PropertyName == nameof(aircraft.PlannedRoutePoints))
                {
                    UpdatePathsPoints();
                }
            }

            return true;
        }

        private void UpdatePathsPoints()
        {
            PathsPoints.Clear();
            foreach (IAircraftItemViewModel aircraft in Aircrafts)
            {
                foreach (Location aircraftPlannedRoutePoint in aircraft.PlannedRoutePoints)
                {
                    PathsPoints.Add(aircraftPlannedRoutePoint);
                }
            }
        }

        private void UpdateTargetPaths()
        {
            PathsToTarget.Clear();
            foreach (IAircraftItemViewModel aircraftItemViewModel in Aircrafts.Where(x => x.PathToTarget.Any()))
            {
                PathsToTarget.Add(aircraftItemViewModel.PathToTarget.First());
            }
        }
    }
}