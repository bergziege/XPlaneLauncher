using System;
using MapControl;
using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Auto.ViewModels.Design {
    public class AutoLogEntryDesignViewModel : IAutoLogEntryViewModel {
        public DelegateCommand BackCommand { get; } = new DelegateCommand(() => { });
        public double? Distance { get; set; } = 42.21;
        public double? Duration { get; set; } = 12.5;
        public DateTime? EndDate { get; set; } = new DateTime(2019, 1, 1, 10, 42, 24);

        public DateTime? EndTime { get; set; } = new DateTime(2020, 1, 1, 12, 21, 42);

        public string Note { get; set; } = "A note";
        public DelegateCommand SaveCommand { get; } = new DelegateCommand(() => { });
        public DateTime? StartDate { get; set; } = new DateTime(2018, 12, 28, 5, 4, 3);

        public DateTime? StartTime { get; set; } = new DateTime(2020, 1, 1, 21, 12, 21);
    }
}