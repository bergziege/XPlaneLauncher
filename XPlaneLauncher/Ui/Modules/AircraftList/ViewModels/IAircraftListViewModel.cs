using System.Collections.ObjectModel;
using Prism.Commands;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels {
    public interface IAircraftListViewModel {
        ObservableCollection<Aircraft> Aircrafts { get; }
        DelegateCommand ReloadCommand { get; }
        Aircraft SelectedAircraft { get; set; }
        DelegateCommand StartSimCommand { get; }
        DelegateCommand ShowSettingsCommand { get; }
        DelegateCommand EditSelectedAircraftRoute { get; }
        bool IsFilteredToMapBoundaries { get; set; }
        DelegateCommand RemoveSelectedAircraftCommand { get; }
    }
}