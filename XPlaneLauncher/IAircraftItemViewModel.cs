using System.Collections.ObjectModel;
using System.ComponentModel;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.ViewModels;

namespace XPlaneLauncher
{
    public interface IAircraftItemViewModel  :INotifyPropertyChanged
    {
        string ThumbnailPath { get; }

        string Name { get; }

        string Livery { get; }

        bool HasSitFile { get; }

        bool HasThumbnail { get; }

        bool IsSelected { get; set; }

        AircraftDto AircraftDto { get; }

        bool IsInTargetSelectionMode { get; }

        DelegateCommand StartTargetSelectionModeCommand { get; }

        DelegateCommand EndTargetSelectionModeCommand { get; }

        Location TargetLocation { get; }
        void UpdateTarget(Location targetLocation);
        DelegateCommand RemoveTargetCommand { get; }
        IAircraftItemViewModel Initialize(AircraftDto aircraft);

        ObservableCollection<Polyline> PathToTarget { get; }

        double? DistanceToDestination { get; }
    }
}