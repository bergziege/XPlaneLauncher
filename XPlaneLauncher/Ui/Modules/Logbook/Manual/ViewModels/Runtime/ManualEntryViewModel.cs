using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Common.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels.Runtime {
    public class ManualEntryViewModel : BindableBase, IManualEntryViewModel {
        private readonly NavigateBackCommand _navigateBackCommand;
        private DelegateCommand _backCommand;

        public ManualEntryViewModel(NavigateBackCommand navigateBackCommand) {
            _navigateBackCommand = navigateBackCommand;
        }

        public DelegateCommand BackCommand {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(OnGoBack)); }
        }

        private void OnGoBack() {
            _navigateBackCommand.Execute();
        }
    }
}