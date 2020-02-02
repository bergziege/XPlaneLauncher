﻿using System;
using MapControl;
using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels.Design {
    public class ManualEntryDesignViewModel : IManualEntryViewModel {
        public DelegateCommand BackCommand { get; } = new DelegateCommand(() => { });
        public double Distance { get; set; } = 42.21;
        public double Duration { get; set; } = 12.5;
        public DateTime EndDateTime { get; set; } = new DateTime(2019, 1, 1, 10, 42, 24);
        public Location EndLocation { get; set; } = new Location(51, 13);
        public bool IsInEndSelectionMode { get; } = true;
        public bool IsInStartSelectionMode { get; } = true;
        public string Note { get; set; } = "A note";
        public DelegateCommand SaveCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand SelectEndLocationCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand SelectStartLocationCommand { get; } = new DelegateCommand(() => { });
        public DateTime StartDateTime { get; set; } = new DateTime(2018, 12, 28, 5, 4, 3);
        public Location StartLocation { get; set; } = new Location(49, 11);
    }
}