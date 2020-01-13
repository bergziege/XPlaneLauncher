using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapControl;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services.Impl {
    public class RouteService : IRouteService {
        public Polyline GetRouteLine(Aircraft aircraft,ObservableCollection<RoutePoint> aircraftRoute) {
            Polyline line = new Polyline();
            //DistanceToDestination = null;
            Location segmentStart = aircraft.Location;
            if (aircraft.Route.Any()) {
                //double distance = 0;
                LocationCollection pathLocations = new LocationCollection();
                foreach (Location segmentEnd in aircraft.Route.Select(x => x.Location)) {
                    //distance += segmentStart.GreatCircleDistance(segmentEnd);
                    pathLocations.AddRange(segmentStart.CalculateGreatCircleLocations(segmentEnd));
                    segmentStart = segmentEnd;
                }


                line.Locations = pathLocations;
                //DistanceToDestination = distance / 1000;
            }

            return line;
        }
    }
}