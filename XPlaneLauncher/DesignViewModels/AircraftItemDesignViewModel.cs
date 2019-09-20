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
        public bool HasSitFile { get; } = true;
        public bool HasThumbnail { get; } 
        public AircraftDto AircraftDto { get; }
        public bool IsInTargetSelectionMode { get; } = true;
        public DelegateCommand StartTargetSelectionModeCommand { get; }
        public DelegateCommand EndTargetSelectionModeCommand { get; }
        public Location TargetLocation { get; } = new Location(15,13);
        public void UpdateTarget(Location targetLocation)
        {
            throw new System.NotImplementedException();
        }

        public DelegateCommand RemoveTargetCommand { get; }
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

        public bool IsSelected { get; set; }
    }
}