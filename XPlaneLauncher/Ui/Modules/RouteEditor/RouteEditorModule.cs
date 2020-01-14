using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands;
using XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels;
using XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.RouteEditor.Views;

namespace XPlaneLauncher.Ui.Modules.RouteEditor {
    public class RouteEditorModule : IModule {
        public void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.Register<RouteEditorNavigationCommand>();
            containerRegistry.Register<IRouteEditorViewModel, RouteEditorViewModel>();
            containerRegistry.RegisterForNavigation<RouteEditorView>();
            ViewModelLocationProvider.Register<RouteEditorView, IRouteEditorViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider) {
        }
    }
}