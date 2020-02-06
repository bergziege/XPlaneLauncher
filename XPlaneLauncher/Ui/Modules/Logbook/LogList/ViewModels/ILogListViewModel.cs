using System.Collections.ObjectModel;
using Prism.Commands;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels {
    public interface ILogListViewModel {
        DelegateCommand AddManualEntryCommand { get; }
        DelegateCommand BackCommand { get; }
        DelegateCommand DeleteSelectedEntryCommand { get; }
        DelegateCommand EditSelectedEntryCommand { get; }
        ObservableCollection<LogbookEntry> LogEntries { get; }
        LogbookEntry SelectedEntry { get; set; }
    }
}