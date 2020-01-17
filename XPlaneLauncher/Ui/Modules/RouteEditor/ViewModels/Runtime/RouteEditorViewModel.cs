using System;
using System.ComponentModel;
using System.Windows;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Services;
using XPlaneLauncher.Ui.Common.Commands;
using XPlaneLauncher.Ui.Modules.Map.Events;
using XPlaneLauncher.Ui.Modules.RouteEditor.Events;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands.Parameters;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Runtime {
    public class RouteEditorViewModel : BindableBase, IRouteEditorViewModel, INavigationAware, IActiveAware, IWeakEventListener {
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
            _eventAggregator.GetEvent<PubSubEvent<LocationSelectedEvent>>().Subscribe(OnLocationSelected);
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

        /// <summary>
        ///     Gets or sets a value indicating whether the object is active.
        /// </summary>
        /// <value><see langword="true" /> if the object is active; otherwise <see langword="false" />.</value>
        public bool IsActive { get; set; }

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

        /// <summary>
        ///     Notifies that the value for <see cref="P:Prism.IActiveAware.IsActive" /> property has changed.
        /// </summary>
        public event EventHandler IsActiveChanged;

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
                foreach (RoutePoint routePoint in Aircraft.Route) {
                    PropertyChangedEventManager.AddListener(routePoint, this, nameof(RoutePoint.IsSelected));
                }
            }
        }

        private bool CanDeleteSelectedRoutePoint() {
            return SelectedRoutePoint != null;
        }

        private void OnDeleteSelectedRoutePoint() {
            Guid routePointId = SelectedRoutePoint.Id;
            _aircraftService.RemoveRoutePointFromAircraft(_aircraft, SelectedRoutePoint);
            SelectedRoutePoint = null;
            _eventAggregator.GetEvent<PubSubEvent<RoutePointRemovedEvent>>().Publish(new RoutePointRemovedEvent(_aircraft.Id, routePointId));
        }

        private void OnLeaveEditor() {
            _navigateBackCommand.Execute();
        }

        private void OnLocationSelected(LocationSelectedEvent obj) {
            if (IsActive) {
                RoutePoint addedRoutePoint = _aircraftService.AddRoutePointToAircraft(_aircraft, obj.Location);
                PropertyChangedEventManager.AddListener(addedRoutePoint, this, nameof(RoutePoint.IsSelected));
                _eventAggregator.GetEvent<PubSubEvent<RoutePointAddedEvent>>().Publish(new RoutePointAddedEvent(_aircraft.Id, addedRoutePoint));
            }
        }

        /// <summary>Empfängt Ereignisse vom zentralen Ereignis-Manager.</summary>
        /// <param name="managerType">
        ///   Der Typ, der die <see cref="T:System.Windows.WeakEventManager" /> Aufrufen dieser Methode.
        /// </param>
        /// <param name="sender">
        ///   Objekt, das das Ereignis ausgelöst wurde.
        /// </param>
        /// <param name="e">Ereignisdaten.</param>
        /// <returns>
        ///   <see langword="true" /> Wenn der Listener das Ereignis behandelt.
        ///    Fehler durch gilt die <see cref="T:System.Windows.WeakEventManager" /> Behandlung in WPF Registrieren eines Listeners für ein Ereignis, das der Listener nicht behandelt wird.
        ///    Die Methode sollte jedoch unabhängig davon zurückgeben <see langword="false" /> wenn ein Ereignis empfangen wird, die nicht erkannt oder behandelt.
        /// </returns>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            if (sender is RoutePoint) {
                SelectedRoutePoint = (RoutePoint)sender;
            }
            return true;
        }
    }
}