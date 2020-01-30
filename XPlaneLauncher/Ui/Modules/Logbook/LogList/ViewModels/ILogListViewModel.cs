using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels {
    public interface ILogListViewModel {
        DelegateCommand BackCommand { get; }
    }
}