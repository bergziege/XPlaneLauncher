using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapControl;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services.Impl {
    public class RouteService : IRouteService {
        public AircraftRouteViewModel GetRouteLine(Aircraft aircraft) {
            AircraftRouteViewModel line = new AircraftRouteViewModel(aircraft.Id);
            Location segmentStart = aircraft.Location;
            if (aircraft.Route.Any()) {
                LocationCollection pathLocations = new LocationCollection();
                foreach (Location segmentEnd in aircraft.Route.Select(x => x.Location)) {
                    pathLocations.AddRange(segmentStart.CalculateGreatCircleLocations(segmentEnd));
                    segmentStart = segmentEnd;
                }

                line.Locations = pathLocations;
            }

            return line;
        }

        public double GetRouteLenght(Aircraft aircraft) {
            double distanceToDestination = 0;
            Location segmentStart = aircraft.Location;
            if (aircraft.Route.Any()) {
                foreach (Location segmentEnd in aircraft.Route.Select(x => x.Location)) {
                    distanceToDestination += segmentStart.GreatCircleDistance(segmentEnd);
                    segmentStart = segmentEnd;
                }

                distanceToDestination = distanceToDestination / 1000;
            }

            return distanceToDestination;
        }
    }
}