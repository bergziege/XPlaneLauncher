using System;
using MapControl;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Map.Events;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels.Runtime {
    public class ManualEntryViewModel : BindableBase, IManualEntryViewModel {
        private readonly NavigateBackCommand _navigateBackCommand;
        private readonly IEventAggregator _eventAggregator;
        private DelegateCommand _backCommand;
        private double _distance;
        private double _duration;
        private DateTime _endDateTime;
        private Location _endLocation;
        private bool _isInEndSelectionMode;
        private bool _isInStartSelectionMode;
        private string _note;
        private DelegateCommand _saveCommand;
        private DelegateCommand _selectEndLocationCommand;
        private DelegateCommand _selectStartLocationCommand;
        private DateTime _startDateTime;
        private Location _startLocation;

        public ManualEntryViewModel(NavigateBackCommand navigateBackCommand, IEventAggregator eventAggregator) {
            _navigateBackCommand = navigateBackCommand;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PubSubEvent<LocationSelectedEvent>>().Subscribe(OnMapLocationChanged);
        }

        private void OnMapLocationChanged(LocationSelectedEvent obj) {
            if (IsInStartSelectionMode) {
                IsInStartSelectionMode = false;
            }else if (IsInEndSelectionMode) {
                IsInEndSelectionMode = false;
            }
        }

        public DelegateCommand BackCommand {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(OnGoBack)); }
        }

        public double Distance {
            get { return _distance; }
            set { _distance = value; }
        }

        public double Duration {
            get { return _duration; }
            set { _duration = value; }
        }

        public DateTime EndDateTime {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }

        public Location EndLocation {
            get { return _endLocation; }
            set { _endLocation = value; }
        }

        public bool IsInEndSelectionMode {
            get { return _isInEndSelectionMode; }
            private set { SetProperty(ref _isInEndSelectionMode, value, nameof(IsInEndSelectionMode));}
        }

        public bool IsInStartSelectionMode {
            get { return _isInStartSelectionMode; }
            private set { SetProperty(ref _isInStartSelectionMode, value, nameof(IsInStartSelectionMode));}
        }

        public string Note {
            get { return _note; }
            set { _note = value; }
        }

        public DelegateCommand SaveCommand {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(OnSave, CanSave)); }
        }

        private bool CanSave() {
            return true;
        }

        private void OnSave() {
            _backCommand.Execute();
        }

        public DelegateCommand SelectEndLocationCommand {
            get { return _selectEndLocationCommand ?? (_selectEndLocationCommand = new DelegateCommand(OnSelectEndLocation)); }
        }

        private void OnSelectEndLocation() {
            IsInEndSelectionMode = true;
        }

        public DelegateCommand SelectStartLocationCommand {
            get { return _selectStartLocationCommand ?? (_selectStartLocationCommand = new DelegateCommand(OnSelectStartLocation)); }
        }

        private void OnSelectStartLocation() {
            IsInStartSelectionMode = true;
        }

        public DateTime StartDateTime {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        public Location StartLocation {
            get { return _startLocation; }
            set { _startLocation = value; }
        }

        private void OnGoBack() {
            _navigateBackCommand.Execute();
        }
    }
}