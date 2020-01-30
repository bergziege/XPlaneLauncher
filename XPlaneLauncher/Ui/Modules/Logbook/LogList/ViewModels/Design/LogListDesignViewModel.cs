using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Design {
    public class LogListDesignViewModel : ILogListViewModel {
        public DelegateCommand BackCommand { get; } = new DelegateCommand(()=>{});
    }
}