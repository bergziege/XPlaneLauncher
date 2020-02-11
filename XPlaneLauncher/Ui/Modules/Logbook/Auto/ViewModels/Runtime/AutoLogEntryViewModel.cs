using System;
using System.IO;
using Ookii.Dialogs.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Services.Impl;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Logbook.Auto.NavigationCommands;
using XPlaneLauncher.Ui.Modules.Logbook.Events;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands.Params;

namespace XPlaneLauncher.Ui.Modules.Logbook.Auto.ViewModels.Runtime {
    public class AutoLogEntryViewModel : BindableBase, IAutoLogEntryViewModel, IRegionMemberLifetime, INavigationAware {
        private readonly IEventAggregator _eventAggregator;
        private readonly LogbookService _logbookService;
        private readonly NavigateBackCommand _navigateBackCommand;
        private DelegateCommand _backCommand;
        private double? _distance;
        private double? _duration;
        private DateTime? _endDateTime;
        private DateTime? _endTime;
        private string _importFile;
        private LogbookEntry _logEntry;
        private string _note;
        private ManualEntryParameters _parameters;
        private DelegateCommand _saveCommand;
        private DateTime? _startDateTime;
        private DateTime? _startTime;

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
                        _importFile = ofd.FileName;
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
            return StartDate.HasValue && StartTime.HasValue && EndDate.HasValue && EndTime.HasValue &&
                   Distance.HasValue &&
                   Duration.HasValue;
        }

        private void OnGoBack() {
            _navigateBackCommand.Execute();
        }

        private void OnSave() {
            _logbookService.CreateAcmiZipEntry(
                new FileInfo(_importFile),
                _parameters.AircraftId,
                StartTime.Value,
                EndTime.Value,
                TimeSpan.FromHours(Duration.Value),
                _logEntry.Track,
                Distance.Value,
                Note);
            _backCommand.Execute();
        }

        private void UpdateData(LogbookEntry entry) {
            StartDate = entry.StartDateTime;
            StartTime = entry.StartDateTime;
            EndDate = entry.EndDateTime;
            EndTime = entry.EndDateTime;
            Distance = entry.DistanceNauticalMiles;
            Duration = entry.Duration.TotalHours;
            Note = entry.Notes;

            VisualizeTrack();
        }

        private void VisualizeTrack() {
            _eventAggregator.GetEvent<PubSubEvent<VisualizeTrackEvent>>()
                .Publish(new VisualizeTrackEvent(_logEntry.Track));
        }
    }
}