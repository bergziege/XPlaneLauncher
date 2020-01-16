using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Services;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.RouteEditor.Events;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands.Parameters;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Runtime {
    public class RouteEditorViewModel : BindableBase, IRouteEditorViewModel, INavigationAware {
        private readonly IAircraftService _aircraftService;
        private readonly IEventAggregator _eventAggregator;
        private readonly NavigateBackCommand _navigateBackCommand;
        private Aircraft _aircraft;
        private DelegateCommand _deleteSelectedRoutePointCommand;
        private DelegateCommand _leaveEditorCommand;
        private RoutePoint _selectedRoutePoint;

        public RouteEditorViewModel(
            NavigateBackCommand navigateBackCommand, IAircraftService aircraftService,
            IEventAggregator eventAggregator) {
            _navigateBackCommand = navigateBackCommand;
            _aircraftService = aircraftService;
            _eventAggregator = eventAggregator;
        }

        public Aircraft Aircraft {
            get { return _aircraft; }
            private set { SetProperty(ref _aircraft, value); }
        }

        public DelegateCommand DeleteSelectedRoutePointCommand {
            get {
                return _deleteSelectedRoutePointCommand ?? (_deleteSelectedRoutePointCommand =
                           new DelegateCommand(OnDeleteSelectedRoutePoint, CanDeleteSelectedRoutePoint));
            }
        }

        public DelegateCommand LeaveEditorCommand {
            get { return _leaveEditorCommand ?? (_leaveEditorCommand = new DelegateCommand(OnLeaveEditor)); }
        }

        public RoutePoint SelectedRoutePoint {
            get { return _selectedRoutePoint; }
            set {
                SetProperty(ref _selectedRoutePoint, value, nameof(SelectedRoutePoint));
                DeleteSelectedRoutePointCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            if (!navigationContext.Parameters.ContainsKey(RouteEditorNavigationCommand.RouteEditorNavParamKey)) {
                return;
            }

            if (navigationContext.Parameters[RouteEditorNavigationCommand.RouteEditorNavParamKey] is RouteEditorNavigationParameters navParams) {
                Aircraft = navParams.Aircraft;
            }
        }

        private bool CanDeleteSelectedRoutePoint() {
            return SelectedRoutePoint != null;
        }

        private void OnDeleteSelectedRoutePoint() {
            _aircraftService.RemoveRoutePoint(_aircraft, SelectedRoutePoint);
            SelectedRoutePoint = null;
            _eventAggregator.GetEvent<PubSubEvent<RoutePointRemovedEvent>>().Publish(new RoutePointRemovedEvent(_aircraft.Id));
        }

        private void OnLeaveEditor() {
            _navigateBackCommand.Execute();
        }
    }
}