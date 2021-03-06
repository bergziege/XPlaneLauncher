﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MapControl;
using Ookii.Dialogs.Wpf;
using Prism.Commands;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Design {
    public class LogListDesignViewModel : ILogListViewModel {
        private DelegateCommand _addAcmiEntryCommand;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogListDesignViewModel() {
            LogEntries = new ObservableCollection<LogbookEntry>();
            LogbookEntry logbookEntry = new LogbookEntry(
                LogbookEntryType.Manual,
                new DateTime(2020, 1, 1, 19, 45, 0),
                new DateTime(2020, 1, 2, 15, 42, 0),
                TimeSpan.FromHours(2.5),
                new List<LogbookLocation>(),
                42.21);
            logbookEntry.Update(
                $"Some multiline{Environment.NewLine}Notes with some more text in it to fill the line and see if its wraps automatically.");
            LogEntries.Add(logbookEntry);
            LogEntries.Add(logbookEntry);
            LogEntries.Add(logbookEntry);
            LogEntries.Add(logbookEntry);
            SelectedEntry = LogEntries[1];
        }

        public DelegateCommand AddAcmiEntryCommand { get; } = new DelegateCommand(()=>{});

        

        public DelegateCommand AddManualEntryCommand { get; } = new DelegateCommand(() => { });

        public DelegateCommand BackCommand { get; } = new DelegateCommand(() => { });

        public DelegateCommand DeleteSelectedEntryCommand { get; } = new DelegateCommand(() => { });

        public DelegateCommand EditSelectedEntryCommand { get; } = new DelegateCommand(() => { });

        public ObservableCollection<LogbookEntry> LogEntries { get; }

        public LogbookEntry SelectedEntry { get; set; }
        public double SummaryDistanceNauticalMiles { get; } = 9842.21;
        public double SummaryDurationHours { get; } = 242.21;
    }
}