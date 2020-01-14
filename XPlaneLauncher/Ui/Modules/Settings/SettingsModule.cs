using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Modules.Settings.ViewCommands;
using XPlaneLauncher.Ui.Modules.Settings.ViewModels;
using XPlaneLauncher.Ui.Modules.Settings.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.Settings.Views;

namespace XPlaneLauncher.Ui.Modules.Settings {
    public class SettingsModule : IModule {
        public void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.Register<ISettingsViewModel, SettingsViewModel>();
            containerRegistry.Register<SettingsNavigationCommand>();
            containerRegistry.RegisterForNavigation<SettingsView>();
            ViewModelLocationProvider.Register<SettingsView, ISettingsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider) {
            
        }
    }
}