using System;
using MapControl;
using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Auto.ViewModels {
    public interface IAutoLogEntryViewModel {
        DelegateCommand BackCommand { get; }
        double? Distance { get; set; }
        double? Duration { get; set; }
        DateTime? EndDate { get; set; }
        Location EndLocation { get; set; }
        DateTime? EndTime { get; set; }
        string Note { get; set; }
        DelegateCommand SaveCommand { get; }
        DateTime? StartDate { get; set; }
        Location StartLocation { get; set; }
        DateTime? StartTime { get; set; }
    }
}