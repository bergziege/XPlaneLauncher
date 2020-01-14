using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands.Parameters;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Runtime {
    public class RouteEditorViewModel : BindableBase, IRouteEditorViewModel, INavigationAware {
        private readonly NavigateBackCommand _navigateBackCommand;
        private Aircraft _aircraft;
        private DelegateCommand _leaveEditorCommand;

        public RouteEditorViewModel(NavigateBackCommand navigateBackCommand) {
            _navigateBackCommand = navigateBackCommand;
        }

        public Aircraft Aircraft {
            get { return _aircraft; }
            private set { SetProperty(ref _aircraft, value); }
        }

        public DelegateCommand LeaveEditorCommand {
            get { return _leaveEditorCommand ?? (_leaveEditorCommand = new DelegateCommand(OnLeaveEditor)); }
        }

        private void OnLeaveEditor() {
            _navigateBackCommand.Execute();
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            if (!navigationContext.Parameters.ContainsKey(RouteEditorNavigationCommand.RouteEditorNavParamKey)) {
                return;
            }

            if (navigationContext.Parameters[RouteEditorNavigationCommand.RouteEditorNavParamKey] is RouteEditorNavigationParameters navParams) {
                Aircraft = navParams.Aircraft;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
        }
    }
}