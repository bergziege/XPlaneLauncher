﻿using System;
using System.Collections.Generic;
using MapControl;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using UnitsNet;
using XPlaneLauncher.Services.Impl;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands.Params;
using XPlaneLauncher.Ui.Modules.Map.Events;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels.Runtime {
    public class ManualEntryViewModel : BindableBase, IManualEntryViewModel, IRegionMemberLifetime, INavigationAware {
        private readonly IEventAggregator _eventAggregator;
        private readonly LogbookService _logbookService;
        private readonly NavigateBackCommand _navigateBackCommand;
        private DelegateCommand _backCommand;
        private double? _distance;
        private double? _duration;
        private DateTime? _endDateTime;
        private Location _endLocation;
        private bool _isInEndSelectionMode;
        private bool _isInStartSelectionMode;
        private string _note;
        private DelegateCommand _saveCommand;
        private DelegateCommand _selectEndLocationCommand;
        private DelegateCommand _selectStartLocationCommand;
        private DateTime? _startDateTime;
        private Location _startLocation;
        private ManualEntryParameters _parameters;

        public ManualEntryViewModel(
            NavigateBackCommand navigateBackCommand, IEventAggregator eventAggregator,
            LogbookService logbookService) {
            _navigateBackCommand = navigateBackCommand;
            _eventAggregator = eventAggregator;
            _logbookService = logbookService;
            _eventAggregator.GetEvent<PubSubEvent<LocationSelectedEvent>>().Subscribe(OnMapLocationChanged);
        }

        public DelegateCommand BackCommand {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(OnGoBack)); }
        }

        public double? Distance {
            get { return _distance; }
            set {
                SetProperty(ref _distance, value, nameof(Distance));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public double? Duration {
            get { return _duration; }
            set {
                SetProperty(ref _duration, value, nameof(Duration));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime? EndDateTime {
            get { return _endDateTime; }
            set {
                SetProperty(ref _endDateTime, value, nameof(EndDateTime));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDuration();
            }
        }

        public Location EndLocation {
            get { return _endLocation; }
            set {
                SetProperty(ref _endLocation, value, nameof(EndLocation));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDistanceInNauticalMiles();
            }
        }

        public bool IsInEndSelectionMode {
            get { return _isInEndSelectionMode; }
            private set { SetProperty(ref _isInEndSelectionMode, value, nameof(IsInEndSelectionMode)); }
        }

        public bool IsInStartSelectionMode {
            get { return _isInStartSelectionMode; }
            private set { SetProperty(ref _isInStartSelectionMode, value, nameof(IsInStartSelectionMode)); }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance should be kept-alive upon deactivation.
        /// </summary>
        public bool KeepAlive { get; } = false;

        public string Note {
            get { return _note; }
            set { SetProperty(ref _note, value, nameof(Note)); }
        }

        public DelegateCommand SaveCommand {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(OnSave, CanSave)); }
        }

        public DelegateCommand SelectEndLocationCommand {
            get { return _selectEndLocationCommand ?? (_selectEndLocationCommand = new DelegateCommand(OnSelectEndLocation)); }
        }

        public DelegateCommand SelectStartLocationCommand {
            get { return _selectStartLocationCommand ?? (_selectStartLocationCommand = new DelegateCommand(OnSelectStartLocation)); }
        }

        public DateTime? StartDateTime {
            get { return _startDateTime; }
            set {
                SetProperty(ref _startDateTime, value, nameof(StartDateTime));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDuration();
            }
        }

        public Location StartLocation {
            get { return _startLocation; }
            set {
                SetProperty(ref _startLocation, value, nameof(StartLocation));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDistanceInNauticalMiles();
            }
        }

        /// <summary>
        ///     Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>
        ///     <see langword="true" /> if this instance accepts the navigation request; otherwise, <see langword="false" />.
        /// </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        /// <summary>
        ///     Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext) {
        }

        /// <summary>Called when the implementer has been navigated to.</summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext) {
            if (navigationContext.Parameters.ContainsKey(ShowManualEntryCommand.NAV_PARAM_KEY)) {
                _parameters = navigationContext.Parameters[ShowManualEntryCommand.NAV_PARAM_KEY] as ManualEntryParameters;
                
            }
        }

        private void CalculateDistanceInNauticalMiles() {
            if (StartLocation == null || EndLocation == null) {
                Distance = null;
            } else {
                Distance = Length.FromMeters(StartLocation.GreatCircleDistance(EndLocation)).NauticalMiles;
            }
        }

        private void CalculateDuration() {
            if (!StartDateTime.HasValue || !EndDateTime.HasValue) {
                Duration = null;
            } else {
                Duration = EndDateTime.Value.Subtract(StartDateTime.Value).TotalHours;
            }
        }

        private bool CanSave() {
            return StartDateTime.HasValue && StartLocation != null && EndDateTime.HasValue && EndLocation != null && Distance.HasValue &&
                   Duration.HasValue;
        }

        private void OnGoBack() {
            _navigateBackCommand.Execute();
        }

        private void OnMapLocationChanged(LocationSelectedEvent obj) {
            if (IsInStartSelectionMode) {
                StartLocation = obj.Location;
                IsInStartSelectionMode = false;
            }

            if (IsInEndSelectionMode) {
                EndLocation = obj.Location;
                IsInEndSelectionMode = false;
            }
        }

        private void OnSave() {
            _logbookService.CreateManualEntry(
                _parameters.AircraftId,
                StartDateTime.Value,
                EndDateTime.Value,
                TimeSpan.FromHours(Duration.Value),
                new List<Location> {
                    StartLocation, EndLocation
                },
                Distance.Value,
                Note);
            _backCommand.Execute();
        }

        private void OnSelectEndLocation() {
            IsInEndSelectionMode = true;
        }

        private void OnSelectStartLocation() {
            IsInStartSelectionMode = true;
        }
    }
}