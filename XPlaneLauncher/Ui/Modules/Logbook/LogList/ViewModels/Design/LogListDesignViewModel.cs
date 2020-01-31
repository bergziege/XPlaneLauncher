using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Design {
    public class LogListDesignViewModel : ILogListViewModel {
        public DelegateCommand AddManualEntryCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand BackCommand { get; } = new DelegateCommand(() => { });
    }
}