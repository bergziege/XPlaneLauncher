using System.Collections.Generic;
using System.Threading.Tasks;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Persistence;

namespace XPlaneLauncher.Services.Impl {
    public class AircraftService : IAircraftService {
        private readonly IAircraftInformationDao _aircraftInformationDao;
        private readonly IAircraftModelProvider _aircraftModelProvider;
        private readonly ISitFileDao _sitFileDao;
        private readonly IThumbnailDao _thumbnailDao;
        private readonly IRouteService _routeService;
        private readonly ILauncherInformationDao _launcherInformationDao;

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
    }
}