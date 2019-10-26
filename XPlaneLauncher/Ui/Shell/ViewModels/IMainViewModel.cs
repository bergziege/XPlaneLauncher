using System.Collections.ObjectModel;
using System.ComponentModel;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Map;

namespace XPlaneLauncher.Ui.Shell.ViewModels {
    public interface IMainViewModel : INotifyPropertyChanged {
        DelegateCommand RefreshCommand { get; }

        DelegateCommand StartSimCommand { get; }

        ObservableCollection<IAircraftItemViewModel> Aircrafts { get; }

        IAircraftItemViewModel SelectedAircraft { get; set; }

        ObservableCollection<AircraftItem> MapAircraftItems { get; }

        Location MapCenter { get; set; }

        bool IsMapInSelectionMode { get; }

        DelegateCommand<Location> ApplyTargetCommand { get; }

        ObservableCollection<Polyline> PathsToTarget { get; }

        ObservableCollection<Location> PathsPoints { get; }

        DelegateCommand UnselectAircraftCommand { get; }

        DelegateCommand ShowSettingsCommand { get; }

        DelegateCommand<MapBoundary> MapBoundariesChangedCommand { get; }

        bool IsListFilteredByMapBoundary { get; set; }
    }
}