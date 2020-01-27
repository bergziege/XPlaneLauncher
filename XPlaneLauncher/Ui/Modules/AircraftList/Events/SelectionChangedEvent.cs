using System;

namespace XPlaneLauncher.Ui.Modules.AircraftList.Events {
    public class SelectionChangedEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public SelectionChangedEvent(Guid? selectedAircraftId) {
            SelectedAircraftId = selectedAircraftId;
        }

        public Guid? SelectedAircraftId { get; }
    }
}