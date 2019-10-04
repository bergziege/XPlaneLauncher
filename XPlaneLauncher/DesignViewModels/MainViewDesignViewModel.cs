using System.Collections.ObjectModel;
using MapControl;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Map;

namespace XPlaneLauncher.DesignViewModels
{
    public class MainViewDesignViewModel : BindableBase, IMainViewModel
    {
        public MainViewDesignViewModel()
        {
            Aircrafts = new ObservableCollection<IAircraftItemViewModel>()
            {
                new AircraftItemDesignViewModel(true, false),
                new AircraftItemDesignViewModel(false, true),
                new AircraftItemDesignViewModel(false, true)
            };

            SelectedAircraft = Aircrafts[1];
        }

        public DelegateCommand RefreshCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand StartSimCommand { get; } = new DelegateCommand(() => { });

        public ObservableCollection<IAircraftItemViewModel> Aircrafts { get; }

        public IAircraftItemViewModel SelectedAircraft { get; set; }

        public ObservableCollection<AircraftItem> MapAircraftItems { get; } = new ObservableCollection<AircraftItem>()
            {new AircraftItem("test", 51, 13, new AircraftItemDesignViewModel(true, true))};

        public Location MapCenter { get; set; } = new Location(51, 13);
        public bool IsMapInSelectionMode { get; } = true;
        public DelegateCommand<Location> ApplyTargetCommand { get; } = new DelegateCommand<Location>((Location loc)=>{});
        public ObservableCollection<Polyline> PathsToTarget { get; } = new ObservableCollection<Polyline>();
        public ObservableCollection<Location> PathsPoints { get; } = new ObservableCollection<Location>();
        public DelegateCommand UnselectAircraftCommand { get; } = new DelegateCommand(()=>{});
    }
}