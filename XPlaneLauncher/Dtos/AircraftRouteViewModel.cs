using System;
using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Dtos {
    public class AircraftRouteViewModel : BindableBase {
        private bool _isSelected;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public AircraftRouteViewModel(Guid aircraftId) {
            AircraftId = aircraftId;
        }

        public Guid AircraftId { get; }

        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value, nameof(IsSelected)); }
        }

        public LocationCollection Locations { get; set; }
    }
}