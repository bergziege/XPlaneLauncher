using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.NavigationComands;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.Views;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.Views;

namespace XPlaneLauncher.Ui.Modules.Logbook {
    public class LogbookModule : IModule {
        /// <summary>Notifies the module that it has been initialized.</summary>
        public void OnInitialized(IContainerProvider containerProvider) {
        }

        /// <summary>
        ///     Used to register types with the container that will be used by your application.
        /// </summary>
        public void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.Register<ShowLogbookForAircraftCommand>();
            containerRegistry.RegisterForNavigation<LogListView>();
            containerRegistry.Register<ILogListViewModel, LogListViewModel>();
            ViewModelLocationProvider.Register<LogListView, ILogListViewModel>();

            containerRegistry.Register<ShowManualEntryCommand>();
            containerRegistry.RegisterForNavigation<ManualLogEntryView>();
            containerRegistry.Register<IManualEntryViewModel, ManualEntryViewModel>();
            ViewModelLocationProvider.Register<ManualLogEntryView, IManualEntryViewModel>();
        }
    }
}