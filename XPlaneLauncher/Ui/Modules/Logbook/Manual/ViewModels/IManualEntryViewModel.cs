using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels {
    public interface IManualEntryViewModel {
        DelegateCommand BackCommand { get; }
    }
}