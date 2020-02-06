using System.Collections.ObjectModel;
using System.IO;
using Prism.Commands;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Design {
    public class AircraftListDesignViewModel : IAircraftListViewModel {
        /// <summary>
        ///   Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public AircraftListDesignViewModel() {
            Aircraft aircraft = new Aircraft(new AircraftInformation() {
                AircraftFile = "test.acf",
                Livery = "my/test/livery",
            }, new AircraftLauncherInformation());
            aircraft.Update(new Situation(true, new FileInfo("temp.tmp")));
            Aircraft aircraft2 = new Aircraft(new AircraftInformation() {
                AircraftFile = "test.acf",
                Livery = "my/test/livery"
            }, new AircraftLauncherInformation());
            aircraft2.Update(new Situation(false, null));
            Aircrafts.Add(aircraft);
            Aircrafts.Add(aircraft2);
            SelectedAircraft = aircraft;
            SelectedAircraft.IsSelected = true;
        }

        public ObservableCollection<Aircraft> Aircrafts { get; } = new ObservableCollection<Aircraft>();
        public DelegateCommand ReloadCommand { get; } = new DelegateCommand(() => { });

        public Aircraft SelectedAircraft { get; set; }

        public DelegateCommand StartSimCommand { get; } = new DelegateCommand(()=>{});

        public DelegateCommand ShowSettingsCommand { get; } = new DelegateCommand(()=>{});

        public DelegateCommand EditSelectedAircraftRoute { get; } = new DelegateCommand(()=>{});
        public bool IsFilteredToMapBoundaries { get; set; }
        public DelegateCommand RemoveSelectedAircraftCommand { get; } = new DelegateCommand(()=>{});
        public DelegateCommand ShowLogbookForSelectedAircraftCommand { get; } = new DelegateCommand(()=>{});
    }
}