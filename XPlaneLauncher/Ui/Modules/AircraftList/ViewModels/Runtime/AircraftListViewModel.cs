using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Services;
using XPlaneLauncher.Ui.Modules.AircraftList.Events;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.Events;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands;
using XPlaneLauncher.Ui.Modules.Settings.ViewCommands;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime {
    public class AircraftListViewModel : BindableBase, IAircraftListViewModel, IWeakEventListener, INavigationAware {
        private readonly IAircraftService _aircraftService;
        private readonly IEventAggregator _eventAggregator;
        private readonly RouteEditorNavigationCommand _routeEditorNavCommand;
        private readonly SettingsNavigationCommand _settingsNavigationCommand;
        private readonly ISettingsService _settingsService;
        private DelegateCommand _editSelectedAircraftRoute;
        private bool _isActive;
        private bool _isFilteredToMapBoundaries;
        private bool _isMarkingSelectedAfterSelectedInList;

        private bool _isSelectingAircraftAfterSelectionChangedInModel;
        private MapBoundary _lastMapBoundaries;
        private DelegateCommand _reloadCommand;
        private Aircraft _selectedAircraft;
        private DelegateCommand _showSettingsCommand;
        private DelegateCommand _startSimCommand;

        public AircraftListViewModel(
            IAircraftService aircraftService, IAircraftModelProvider aircraftModelProvider, SettingsNavigationCommand settingsNavigationCommand,
            RouteEditorNavigationCommand routeEditorNavCommand,
            IEventAggregator eventAggregator,
            ISettingsService settingsService) {
            _aircraftService = aircraftService;
            _settingsNavigationCommand = settingsNavigationCommand;
            _routeEditorNavCommand = routeEditorNavCommand;
            _eventAggregator = eventAggregator;
            _settingsService = settingsService;
            Aircrafts = aircraftModelProvider.Aircrafts;
            CollectionChangedEventManager.AddListener(Aircrafts, this);
            _eventAggregator.GetEvent<PubSubEvent<MapBoundariesChangedEvent>>().Subscribe(OnMapBoundariesChanged);
        }

        public ObservableCollection<Aircraft> Aircrafts { get; }

        public DelegateCommand EditSelectedAircraftRoute {
            get {
                return _editSelectedAircraftRoute ?? (_editSelectedAircraftRoute = new DelegateCommand(
                           OnEditSelectedAircraftRoute));
            }
        }

        public bool IsFilteredToMapBoundaries {
            get { return _isFilteredToMapBoundaries; }
            set {
                SetProperty(ref _isFilteredToMapBoundaries, value, nameof(IsFilteredToMapBoundaries));
                if (_isFilteredToMapBoundaries) {
                    FilterByMapBoundary(_lastMapBoundaries);
                } else {
                    Parallel.ForEach(Aircrafts, aircraft => { aircraft.IsVisible = true; });
                }
            }
        }

        public DelegateCommand ReloadCommand {
            get { return _reloadCommand ?? (_reloadCommand = new DelegateCommand(Reload, CanReload)); }
        }

        public Aircraft SelectedAircraft {
            get { return _selectedAircraft; }
            set {
                SetProperty(ref _selectedAircraft, value);
                if (!_isSelectingAircraftAfterSelectionChangedInModel) {
                    MarkSelected();
                    _eventAggregator.GetEvent<PubSubEvent<SelectionChangedEvent>>().Publish(new SelectionChangedEvent(SelectedAircraft?.Id));
                } else {
                    RaisePropertyChanged();
                }

                StartSimCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ShowSettingsCommand {
            get { return _showSettingsCommand ?? (_showSettingsCommand = new DelegateCommand(OnShowSettings)); }
        }

        public DelegateCommand StartSimCommand {
            get { return _startSimCommand ?? (_startSimCommand = new DelegateCommand(StartSim, CanStartSim)); }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            if (managerType == typeof(CollectionChangedEventManager)) {
                Parallel.ForEach(
                    Aircrafts,
                    aircraft => { PropertyChangedEventManager.AddListener(aircraft, this, nameof(aircraft.IsSelected)); });
            } else if (!_isMarkingSelectedAfterSelectedInList) {
                _isSelectingAircraftAfterSelectionChangedInModel = true;
                SelectedAircraft = Aircrafts.FirstOrDefault(x => x.IsSelected);
                _isSelectingAircraftAfterSelectionChangedInModel = false;
            }

            return true;
        }

        private bool CanReload() {
            return true;
        }

        private bool CanStartSim() {
            return SelectedAircraft != null && SelectedAircraft.Situation.HasSit;
        }

        public void CheckSettings() {
            if (!_settingsService.IsHavingRequiredDirectories(
                Properties.Settings.Default.XPlaneRootPath,
                Properties.Settings.Default.DataPath,
                out IList<string> errors)) {
                _settingsNavigationCommand.Execute();
            }
        }

        private void FilterByMapBoundary(MapBoundary mapBoundaries) {
            Parallel.ForEach(
                Aircrafts,
                aircraft => { aircraft.IsVisible = IsWithinMapBoundaries(aircraft, mapBoundaries); });
        }

        private bool IsWithinMapBoundaries(Aircraft aircraft, MapBoundary mapBoundaries) {
            if (aircraft.Location == null) {
                return false;
            }

            return aircraft.Location.Longitude >= mapBoundaries.TopLeft.Longitude && aircraft.Location.Longitude <=
                                                                                  mapBoundaries.BottomRight.Longitude
                                                                                  && aircraft.Location.Latitude <= mapBoundaries.TopLeft.Latitude &&
                                                                                  aircraft.Location.Latitude >= mapBoundaries.BottomRight.Latitude;
        }

        private void MarkSelected() {
            _isMarkingSelectedAfterSelectedInList = true;
            Parallel.ForEach(
                Aircrafts,
                aircraft => { aircraft.IsSelected = false; });

            if (SelectedAircraft != null) {
                SelectedAircraft.IsSelected = true;
            }

            _isMarkingSelectedAfterSelectedInList = false;
        }

        private void OnEditSelectedAircraftRoute() {
            _eventAggregator.GetEvent<PubSubEvent<SelectionChangedEvent>>().Publish(new SelectionChangedEvent(SelectedAircraft.Id));
            _routeEditorNavCommand.Execute(SelectedAircraft);
        }

        private void OnMapBoundariesChanged(MapBoundariesChangedEvent obj) {
            _lastMapBoundaries = obj.MapBoundary;
            if (IsFilteredToMapBoundaries) {
                FilterByMapBoundary(_lastMapBoundaries);
            }
        }

        private void OnShowSettings() {
            _settingsNavigationCommand.Execute();
        }

        private async void Reload() {
            await _aircraftService.ReloadAsync();
            _eventAggregator.GetEvent<PubSubEvent<AircraftsLoadedEvent>>().Publish(new AircraftsLoadedEvent());
        }

        private void StartSim() {
            string xplaneExecutable =
                Path.Combine(Properties.Settings.Default.XPlaneRootPath, Properties.Settings.Default.XPlaneExecutableFile);
            SelectedAircraft.Situation.SitFile.CopyTo(
                Path.Combine(
                    Properties.Settings.Default.XPlaneRootPath,
                    @"Output/Situations/default.sit"),
                true);
            FileInfo executable = new FileInfo(xplaneExecutable);
            if (executable.Exists) {
                Process.Start(xplaneExecutable);
            }
        }

        /// <summary>Called when the implementer has been navigated to.</summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext) {
            CheckSettings();
        }

        /// <summary>
        /// Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>
        /// <see langword="true" /> if this instance accepts the navigation request; otherwise, <see langword="false" />.
        /// </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        /// <summary>
        /// Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext) {
            
        }
    }
}