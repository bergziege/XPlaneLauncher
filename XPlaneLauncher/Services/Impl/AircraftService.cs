using System.Collections.Generic;
using System.Threading.Tasks;
using MapControl;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Persistence;

namespace XPlaneLauncher.Services.Impl {
    public class AircraftService : IAircraftService {
        private readonly IAircraftInformationDao _aircraftInformationDao;
        private readonly IAircraftModelProvider _aircraftModelProvider;
        private readonly ILauncherInformationDao _launcherInformationDao;
        private readonly IRouteService _routeService;
        private readonly ISitFileDao _sitFileDao;
        private readonly IThumbnailDao _thumbnailDao;

        public AircraftService(
            IAircraftInformationDao aircraftInformationDao, ILauncherInformationDao launcherInformationDao,
            IAircraftModelProvider aircraftModelProvider, ISitFileDao sitFileDao, IThumbnailDao thumbnailDao, IRouteService routeService) {
            _aircraftInformationDao = aircraftInformationDao;
            _launcherInformationDao = launcherInformationDao;
            _aircraftModelProvider = aircraftModelProvider;
            _sitFileDao = sitFileDao;
            _thumbnailDao = thumbnailDao;
            _routeService = routeService;
        }

        public RoutePoint AddRoutePointToAircraft(Aircraft aircraft, Location location) {
            RoutePoint routePoint = new RoutePoint(location);
            aircraft.Route.Add(routePoint);
            aircraft.Update(_routeService.GetRouteLenght(aircraft));
            return routePoint;
        }

        public async Task ReloadAsync() {
            _aircraftModelProvider.Aircrafts.Clear();
            IList<AircraftInformation> aircraftInformations = await _aircraftInformationDao.GetAllAsync();
            foreach (AircraftInformation aircraftInformation in aircraftInformations) {
                AircraftLauncherInformation aircraftLauncherInformation = await _launcherInformationDao.FindForAircraftAsync(aircraftInformation);
                Aircraft aircraft = new Aircraft(aircraftInformation, aircraftLauncherInformation);
                aircraft.Init();
                aircraft.Update(_sitFileDao.FindSit(aircraftInformation));
                aircraft.Update(_thumbnailDao.FindThumbnail(aircraftInformation));
                aircraft.Update(_routeService.GetRouteLenght(aircraft));
                _aircraftModelProvider.Aircrafts.Add(aircraft);
            }
        }

        public void RemoveRoutePointFromAircraft(Aircraft aircraft, RoutePoint routePoint) {
            aircraft.Route.Remove(routePoint);
            aircraft.Update(_routeService.GetRouteLenght(aircraft));
        }

        public void Save(Aircraft aircraft) {
            AircraftLauncherInformation launcherInfo = new AircraftLauncherInformation {
                TargetLocation = new List<Location>(),
                LocationInformations = new Dictionary<Location, LocationInformation>()
            };
            foreach (RoutePoint routePoint in aircraft.Route) {
                launcherInfo.TargetLocation.Add(routePoint.Location);
                if (!string.IsNullOrWhiteSpace(routePoint.Name) || !string.IsNullOrWhiteSpace(routePoint.Description)) {
                    launcherInfo.LocationInformations.Add(
                        routePoint.Location,
                        new LocationInformation {
                            Name = routePoint.Name,
                            Comment = routePoint.Description
                        });
                }
            }

            _launcherInformationDao.SaveToFile(aircraft.LauncherInfoFile, launcherInfo);
        }
    }
}