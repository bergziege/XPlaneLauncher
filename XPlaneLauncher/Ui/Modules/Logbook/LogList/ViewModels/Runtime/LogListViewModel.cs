using Prism.Commands;
using XPlaneLauncher.Ui.Common.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Runtime {
    public class LogListViewModel : ILogListViewModel {
        private readonly NavigateBackCommand _navigateBackCommand;
        private DelegateCommand _backCommand;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogListViewModel(NavigateBackCommand navigateBackCommand) {
            _navigateBackCommand = navigateBackCommand;
        }

        public DelegateCommand BackCommand {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(GoBack)); }
        }

        private void GoBack() {
            _navigateBackCommand.Execute();
        }
    }
}