﻿using System.Collections.ObjectModel;
using Prism.Commands;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels {
    public interface ILogListViewModel {
        DelegateCommand AddAcmiEntryCommand { get; }
        DelegateCommand AddManualEntryCommand { get; }
        DelegateCommand BackCommand { get; }
        DelegateCommand DeleteSelectedEntryCommand { get; }
        DelegateCommand EditSelectedEntryCommand { get; }
        ObservableCollection<LogbookEntry> LogEntries { get; }
        LogbookEntry SelectedEntry { get; set; }
        double SummaryDistanceNauticalMiles { get; }
        double SummaryDurationHours { get; }
    }
}