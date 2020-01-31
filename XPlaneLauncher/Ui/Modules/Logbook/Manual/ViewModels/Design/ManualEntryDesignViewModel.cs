using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels.Design {
    public class ManualEntryDesignViewModel : IManualEntryViewModel {
        public DelegateCommand BackCommand { get; } = new DelegateCommand(() => { });
    }
}