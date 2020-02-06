﻿using System;
using MapControl;
using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels {
    public interface IManualEntryViewModel {
        DelegateCommand BackCommand { get; }
        double? Distance { get; set; }
        double? Duration { get; set; }
        DateTime? EndDate { get; set; }
        Location EndLocation { get; set; }
        DateTime? EndTime { get; set; }
        bool IsInEndSelectionMode { get; }
        bool IsInStartSelectionMode { get; }
        string Note { get; set; }
        DelegateCommand SaveCommand { get; }
        DelegateCommand SelectEndLocationCommand { get; }
        DelegateCommand SelectStartLocationCommand { get; }
        DateTime? StartDate { get; set; }
        Location StartLocation { get; set; }
        DateTime? StartTime { get; set; }
    }
}