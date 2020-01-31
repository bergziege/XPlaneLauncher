using Prism.Commands;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.NavigationComands;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Runtime {
    public class LogListViewModel : ILogListViewModel, INavigationAware {
        private readonly NavigateBackCommand _navigateBackCommand;
        private readonly ShowManualEntryCommand _showManualEntryCommand;
        private DelegateCommand _addManualEntryCommand;
        private Aircraft _aircraft;
        private DelegateCommand _backCommand;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogListViewModel(NavigateBackCommand navigateBackCommand, ShowManualEntryCommand showManualEntryCommand) {
            _navigateBackCommand = navigateBackCommand;
            _showManualEntryCommand = showManualEntryCommand;
        }

        public DelegateCommand AddManualEntryCommand {
            get { return _addManualEntryCommand ?? (_addManualEntryCommand = new DelegateCommand(OnAddManualEntry)); }
        }

        public DelegateCommand BackCommand {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(GoBack)); }
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
        }

        private void GoBack() {
            _navigateBackCommand.Execute();
        }

        private void OnAddManualEntry() {
            _showManualEntryCommand.Execute();
        }
    }
}