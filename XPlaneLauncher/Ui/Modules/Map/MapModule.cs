using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Ui.Modules.Map.ViewModels;
using XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.Map.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.Map {
    public class MapModule : IModule {
        private readonly IRegionManager _regionManager;

        public MapModule(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider) {
            _regionManager.RegisterViewWithRegion(RegionNames.MapRegion, typeof(MapView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.Register<IMapViewModel, MapViewModel>();
            ViewModelLocationProvider.Register<MapView, IMapViewModel>();
        }
    }
}