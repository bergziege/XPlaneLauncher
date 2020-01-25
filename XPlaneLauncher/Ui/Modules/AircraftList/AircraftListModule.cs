using System;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Ui.Modules.AircraftList.DialogCommands;
using XPlaneLauncher.Ui.Modules.AircraftList.ViewModels;
using XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.AircraftList.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.AircraftList {
    public class AircraftListModule : IModule {
        private readonly IRegionManager _regionManager;

        public AircraftListModule(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.Register<IAircraftListViewModel, AircraftListViewModel>();
            containerRegistry.Register<ShowRemoveConfirmationDialogCommand>();
            containerRegistry.RegisterForNavigation<AircraftListView>();
            ViewModelLocationProvider.Register<AircraftListView, IAircraftListViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider) { 
            _regionManager.RequestNavigate(RegionNames.AppRegion, new Uri(nameof(AircraftListView), UriKind.Relative));
        }
    }
}