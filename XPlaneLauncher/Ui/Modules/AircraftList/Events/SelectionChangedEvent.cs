using System;

namespace XPlaneLauncher.Ui.Modules.AircraftList.Events {
    public class SelectionChangedEvent {
        private readonly Guid? _selectedAircraftId;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public SelectionChangedEvent(Guid? selectedAircraftId) {
            _selectedAircraftId = selectedAircraftId;
        }
    }
}