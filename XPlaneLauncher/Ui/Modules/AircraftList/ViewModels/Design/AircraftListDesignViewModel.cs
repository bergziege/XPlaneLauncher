using System.Collections.ObjectModel;
using Prism.Commands;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Design {
    public class AircraftListDesignViewModel : IAircraftListViewModel {
        public ObservableCollection<Aircraft> Aircrafts { get; } = new ObservableCollection<Aircraft>();
        public DelegateCommand ReloadCommand { get; } = new DelegateCommand(() => { });

        public Aircraft SelectedAircraft { get; set; }

        public DelegateCommand StartSimCommand { get; } = new DelegateCommand(()=>{});

        public DelegateCommand ShowSettingsCommand { get; } = new DelegateCommand(()=>{});

        public DelegateCommand EditSelectedAircraftRoute { get; } = new DelegateCommand(()=>{});
        public bool IsFilteredToMapBoundaries { get; set; }
        public DelegateCommand RemoveSelectedAircraftCommand { get; } = new DelegateCommand(()=>{});
    }
}