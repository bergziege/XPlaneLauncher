using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime {
    public class AircraftListViewModel : BindableBase, IAircraftListViewModel, IWeakEventListener {
        private readonly IAircraftService _aircraftService;
        private DelegateCommand _reloadCommand;
        private Aircraft _selectedAircraft;
        private DelegateCommand _startSimCommand;

        public AircraftListViewModel(IAircraftService aircraftService, IAircraftModelProvider aircraftModelProvider) {
            _aircraftService = aircraftService;
            Aircrafts = aircraftModelProvider.Aircrafts;
            CollectionChangedEventManager.AddListener(Aircrafts, this);
        }

        public ObservableCollection<Aircraft> Aircrafts { get; }

        public DelegateCommand ReloadCommand {
            get { return _reloadCommand ?? (_reloadCommand = new DelegateCommand(Reload, CanReload)); }
        }

        public Aircraft SelectedAircraft {
            get { return _selectedAircraft; }
            set {
                SetProperty(ref _selectedAircraft, value);
                if (!_isSelectingAircraftAfterSelectionChangedInModel) {
                    MarkSelected();
                } else {
                    RaisePropertyChanged();
                }
                StartSimCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand StartSimCommand {
            get { return _startSimCommand ?? (_startSimCommand = new DelegateCommand(StartSim, CanStartSim)); }
        }

        private void MarkSelected() {
            foreach (Aircraft aircraft in Aircrafts.Where(x => x.IsSelected)) {
                aircraft.IsSelected = false;
            }

            if (SelectedAircraft != null) {
                SelectedAircraft.IsSelected = true;
            }
        }

        private void StartSim() {
            string xplaneExecutable =
                Path.Combine(Properties.Settings.Default.XPlaneRootPath, Properties.Settings.Default.XPlaneExecutableFile);
            SelectedAircraft.Situation.SitFile.CopyTo(
                Path.Combine(
                    Properties.Settings.Default.XPlaneRootPath,
                    @"Output/Situations/default.sit"),
                true);
            FileInfo executable = new FileInfo(xplaneExecutable);
            if (executable.Exists) {
                Process.Start(xplaneExecutable);
            }
        }

        private bool CanStartSim() {
            return SelectedAircraft != null && SelectedAircraft.Situation.HasSit;
        }

        private bool CanReload() {
            return true;
        }

        private async void Reload() {
            await _aircraftService.ReloadAsync();
        }

        private bool _isSelectingAircraftAfterSelectionChangedInModel;

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            if (managerType == typeof(CollectionChangedEventManager)) {
                foreach (Aircraft aircraft in Aircrafts) {
                    PropertyChangedEventManager.AddListener(aircraft, this, nameof(aircraft.IsSelected));
                }
            } else {
                _isSelectingAircraftAfterSelectionChangedInModel = true;
                SelectedAircraft = Aircrafts.FirstOrDefault(x => x.IsSelected);
                _isSelectingAircraftAfterSelectionChangedInModel = false;
            }
            return true;
        }
    }
}