using System.Collections.Generic;
using System.Linq;
using MapControl;
using Prism.Commands;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Design {
    public class RouteEditorDesignViewModel : IRouteEditorViewModel {
        /// <summary>
        ///   Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public RouteEditorDesignViewModel() {
            Location loc = new Location(1,1);
            Aircraft = new Aircraft(new AircraftInformation() {
                AircraftFile = "aircraft.acf",
                Livery = "livery/livery"
            }, new AircraftLauncherInformation() {
                TargetLocation = new List<Location> {loc},
                LocationInformations = new Dictionary<Location, LocationInformation>() {
                    {loc, new LocationInformation(){Name = "Name", Comment = "Comment"} }
                }
            });
            Aircraft.Init();
            SelectedRoutePoint = Aircraft.Route.Single();
            SelectedRoutePoint.IsSelected = true;
        }

        public Aircraft Aircraft { get; } = new Aircraft(new AircraftInformation(), new AircraftLauncherInformation());

        public DelegateCommand LeaveEditorCommand { get; } = new DelegateCommand(()=>{});
        public RoutePoint SelectedRoutePoint { get; set; }
        public DelegateCommand DeleteSelectedRoutePointCommand { get; } = new DelegateCommand(()=>{});
    }
}