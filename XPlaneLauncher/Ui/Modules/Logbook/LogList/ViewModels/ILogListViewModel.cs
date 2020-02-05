using System.Collections.ObjectModel;
using Prism.Commands;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels {
    public interface ILogListViewModel {
        DelegateCommand BackCommand { get; }
        DelegateCommand AddManualEntryCommand { get; }
        ObservableCollection<LogbookEntry> LogEntries { get; }
    }
}