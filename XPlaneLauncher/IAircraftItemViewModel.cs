using System;
using System.Collections.Generic;
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

        ObservableCollection<Location> PlannedRoutePoints { get; }
        void AddToPlannedRoute(Location location);
        DelegateCommand RemoveSelectedRouteLocationCommand { get; }
        IAircraftItemViewModel Initialize(AircraftDto aircraft);

        ObservableCollection<Polyline> PathToTarget { get; }

        double? DistanceToDestination { get; }

        Location SelectedPlannedRoutePoint { get; set; }
    }
}