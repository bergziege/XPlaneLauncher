using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Model.Provider.Impl;
using XPlaneLauncher.Persistence;
using XPlaneLauncher.Persistence.Impl;
using XPlaneLauncher.Services;
using XPlaneLauncher.Services.Impl;
using XPlaneLauncher.Ui.Modules.AircraftList;
using XPlaneLauncher.Ui.Shell.Views;

namespace XPlaneLauncher {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<AircraftListModule>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton<IAircraftModelProvider, AircraftModelProvider>();
            containerRegistry.Register<IAircraftInformationDao, AircraftInformationDao>();
            containerRegistry.Register<ILauncherInformationDao, LauncherInformationDao>();
            containerRegistry.Register<ISitFileDao, SitFileDao>();
            containerRegistry.Register<IThumbnailDao, ThumbnailDao>();
            containerRegistry.Register<IAircraftService, AircraftService>();
        }

        protected override Window CreateShell() {
            return Container.Resolve<MainWindow>();
        }
    }
}