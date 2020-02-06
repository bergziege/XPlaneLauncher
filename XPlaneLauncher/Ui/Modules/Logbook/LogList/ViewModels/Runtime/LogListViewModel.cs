using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;
using XPlaneLauncher.Services;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.NavigationComands;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Runtime {
    public class LogListViewModel : BindableBase, ILogListViewModel, INavigationAware {
        private readonly ILogbookService _logbookService;
        private readonly NavigateBackCommand _navigateBackCommand;
        private readonly ShowManualEntryCommand _showManualEntryCommand;
        private DelegateCommand _addManualEntryCommand;
        private Aircraft _aircraft;
        private DelegateCommand _backCommand;
        private DelegateCommand _deleteSelectedEntryCommand;
        private DelegateCommand _editSelectedEntryCommand;
        private LogbookEntry _selectedEntry;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogListViewModel(
            NavigateBackCommand navigateBackCommand, ShowManualEntryCommand showManualEntryCommand, ILogbookService logbookService) {
            _navigateBackCommand = navigateBackCommand;
            _showManualEntryCommand = showManualEntryCommand;
            _logbookService = logbookService;
        }

        public DelegateCommand AddManualEntryCommand {
            get { return _addManualEntryCommand ?? (_addManualEntryCommand = new DelegateCommand(OnAddManualEntry)); }
        }

        public DelegateCommand BackCommand {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(GoBack)); }
        }

        public DelegateCommand DeleteSelectedEntryCommand {
            get { return _deleteSelectedEntryCommand ?? (_deleteSelectedEntryCommand = new DelegateCommand(OnDeleteSelectedEntry)); }
        }

        public DelegateCommand EditSelectedEntryCommand {
            get { return _editSelectedEntryCommand ?? (_editSelectedEntryCommand = new DelegateCommand(OnEditSelectedEntry)); }
        }

        public ObservableCollection<LogbookEntry> LogEntries { get; } = new ObservableCollection<LogbookEntry>();

        public LogbookEntry SelectedEntry {
            get { return _selectedEntry; }
            set { SetProperty(ref _selectedEntry, value, nameof(SelectedEntry)); }
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
            if (navigationContext.Parameters.ContainsKey(ShowLogbookForAircraftCommand.PARAM_KEY)) {
                if (navigationContext.Parameters[ShowLogbookForAircraftCommand.PARAM_KEY] is Aircraft aircraft) {
                    _aircraft = aircraft;
                }
            }

            RefreshData();
        }

        private void GoBack() {
            _navigateBackCommand.Execute();
        }

        private void OnAddManualEntry() {
            _showManualEntryCommand.Execute(_aircraft.Id, null);
        }

        private void OnDeleteSelectedEntry() {
            _logbookService.DeleteEntry(_aircraft.Id, SelectedEntry);
            LogEntries.Remove(SelectedEntry);
            SelectedEntry = null;
        }

        private void OnEditSelectedEntry() {
            if (SelectedEntry.Type == LogbookEntryType.Manual) {
                _logbookService.ExpandTrack(_aircraft.Id, SelectedEntry);
                _showManualEntryCommand.Execute(_aircraft.Id, SelectedEntry);
            }
        }

        private async void RefreshData() {
            LogEntries.Clear();
            IList<LogbookEntry> entries = await _logbookService.GetEntriesWithoutTrackAsync(_aircraft);
            foreach (LogbookEntry logbookEntry in entries) {
                LogEntries.Add(logbookEntry);
            }
        }
    }
}