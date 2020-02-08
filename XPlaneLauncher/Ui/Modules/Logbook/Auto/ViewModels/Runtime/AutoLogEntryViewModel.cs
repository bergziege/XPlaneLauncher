﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapControl;
using Ookii.Dialogs.Wpf;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using UnitsNet;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Services.Impl;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Logbook.Auto.NavigationCommands;
using XPlaneLauncher.Ui.Modules.Logbook.Events;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands.Params;
using XPlaneLauncher.Ui.Modules.Map.Events;

namespace XPlaneLauncher.Ui.Modules.Logbook.Auto.ViewModels.Runtime {
    public class AutoLogEntryViewModel : BindableBase, IAutoLogEntryViewModel, IRegionMemberLifetime, INavigationAware {
        private readonly IEventAggregator _eventAggregator;
        private readonly LogbookService _logbookService;
        private readonly NavigateBackCommand _navigateBackCommand;
        private DelegateCommand _backCommand;
        private double? _distance;
        private double? _duration;
        private DateTime? _endDateTime;
        private Location _endLocation;
        private DateTime? _endTime;
        private bool _isInEndSelectionMode;
        private bool _isInStartSelectionMode;
        private string _note;
        private ManualEntryParameters _parameters;
        private DelegateCommand _saveCommand;
        private DelegateCommand _selectEndLocationCommand;
        private DelegateCommand _selectStartLocationCommand;
        private DateTime? _startDateTime;
        private Location _startLocation;
        private DateTime? _startTime;
        private LogbookEntry _logEntry;

        public AutoLogEntryViewModel(
            NavigateBackCommand navigateBackCommand, IEventAggregator eventAggregator,
            LogbookService logbookService) {
            _navigateBackCommand = navigateBackCommand;
            _eventAggregator = eventAggregator;
            _logbookService = logbookService;
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

        public DateTime? EndDate {
            get { return _endDateTime; }
            set {
                SetProperty(ref _endDateTime, value, nameof(EndDate));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDuration();
            }
        }

        public Location EndLocation {
            get { return _endLocation; }
            set {
                SetProperty(ref _endLocation, value, nameof(EndLocation));
                SaveCommand.RaiseCanExecuteChanged();
                VisualizeTrack();
            }
        }

        public DateTime? EndTime {
            get { return _endTime; }
            set {
                SetProperty(ref _endTime, value, nameof(EndTime));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDuration();
            }
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

        public DateTime? StartDate {
            get { return _startDateTime; }
            set {
                SetProperty(ref _startDateTime, value, nameof(StartDate));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDuration();
            }
        }

        public Location StartLocation {
            get { return _startLocation; }
            set {
                SetProperty(ref _startLocation, value, nameof(StartLocation));
                SaveCommand.RaiseCanExecuteChanged();
                VisualizeTrack();
            }
        }

        public DateTime? StartTime {
            get { return _startTime; }
            set {
                SetProperty(ref _startTime, value, nameof(StartTime));
                SaveCommand.RaiseCanExecuteChanged();
                CalculateDuration();
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
            if (navigationContext.Parameters.ContainsKey(ShowAutoEntryCommand.NAV_PARAM_KEY)) {
                _parameters = navigationContext.Parameters[ShowAutoEntryCommand.NAV_PARAM_KEY] as ManualEntryParameters;
                if (_parameters?.LogbookEntry != null) {
                    _logEntry = _parameters.LogbookEntry;
                    UpdateData(_parameters.LogbookEntry);
                } else {
                    VistaOpenFileDialog ofd = new VistaOpenFileDialog();
                    bool? dlgResult = ofd.ShowDialog();
                    if (dlgResult.HasValue && dlgResult.Value) {
                        _logEntry = _logbookService.GetEntryFromAcmiFile(new FileInfo(ofd.FileName));
                        UpdateData(_logEntry);
                    }
                }
            }
        }

        private DateTime AddTime(DateTime startDate, DateTime startTime) {
            startDate = startDate.Date.AddHours(startTime.Hour);
            startDate = startDate.AddMinutes(startTime.Minute);
            return startDate;
        }

        private void CalculateDuration() {
            if (!StartDate.HasValue || !StartTime.HasValue || !EndDate.HasValue || !EndTime.HasValue) {
                Duration = null;
            } else {
                Duration = CalculateDurationFromStartAndEnd();
            }
        }

        private double CalculateDurationFromStartAndEnd() {
            DateTime startDateTime = AddTime(StartDate.Value, StartTime.Value);
            DateTime endDateTime = AddTime(EndDate.Value, EndTime.Value);
            return endDateTime.Subtract(startDateTime).TotalHours;
        }

        private bool CanSave() {
            return StartDate.HasValue && StartTime.HasValue && StartLocation != null && EndDate.HasValue && EndTime.HasValue && EndLocation != null &&
                   Distance.HasValue &&
                   Duration.HasValue;
        }

        private void OnGoBack() {
            _navigateBackCommand.Execute();
        }

        private void OnSave() {
            
            _backCommand.Execute();
        }

        private void UpdateData(LogbookEntry entry) {
            StartDate = entry.StartDateTime;
            StartTime = entry.StartDateTime;
            EndDate = entry.EndDateTime;
            EndTime = entry.EndDateTime;
            StartLocation = entry.Track.First();
            EndLocation = entry.Track.Last();
            Distance = entry.DistanceNauticalMiles;
            Duration = entry.Duration.TotalHours;
            Note = entry.Notes;
        }

        private void VisualizeTrack() {
            if (StartLocation != null && EndLocation != null) {
                _eventAggregator.GetEvent<PubSubEvent<VisualizeTrackEvent>>()
                    .Publish(new VisualizeTrackEvent(_logEntry.Track));
            }
        }
    }
}