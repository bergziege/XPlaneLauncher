using System.Collections.Generic;
using System.Collections.ObjectModel;
using MapControl;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.ViewModels;

namespace XPlaneLauncher.DesignViewModels
{
    public class AircraftItemDesignViewModel : BindableBase, IAircraftItemViewModel
    {
        public AircraftItemDesignViewModel(bool hasThumbnail, bool isSelected)
        {
            HasThumbnail = hasThumbnail;
            IsSelected = isSelected;
        }

        public string ThumbnailPath { get; } = @"C:\Users\bernd\Pictures\P1210319.jpg";
        public string Name { get; } = "testflieger mit einem doch für einen flieger relativ langen namen und da ist noch nichtmal die livery dabei";
        public string Livery { get; } = "Lufthansa";
        public string LastPartOfLiveryPath { get; } = "the real livery without path";
        public bool HasSitFile { get; } = true;
        public bool HasThumbnail { get; } 
        public AircraftDto AircraftDto { get; }
        public bool IsInTargetSelectionMode { get; } = true;
        public DelegateCommand StartTargetSelectionModeCommand { get; }
        public DelegateCommand EndTargetSelectionModeCommand { get; }
        public ObservableCollection<Location> PlannedRoutePoints { get; } = new ObservableCollection<Location>(){ new Location(15, 13) };
        public void AddToPlannedRoute(Location location)
        {
            throw new System.NotImplementedException();
        }

        public DelegateCommand RemoveSelectedRouteLocationCommand { get; }
        public IAircraftItemViewModel Initialize(AircraftDto aircraft)
        {
            throw new System.NotImplementedException();
        }

        public ObservableCollection<Polyline> PathToTarget { get; } = new ObservableCollection<Polyline>()
        {
            new Polyline()
            {
                Locations = new LocationCollection { new Location(51,13),
                    new Location(0,0)}
            }
        };

        public double? DistanceToDestination { get; } = 42000;
        public Location SelectedPlannedRoutePoint { get; set; }

        public bool IsSelected { get; set; }
    }
}