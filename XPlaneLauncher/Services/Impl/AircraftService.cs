using System.Collections.Generic;
using System.Threading.Tasks;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;
using XPlaneLauncher.Persistence;
using XPlaneLauncher.Repositories;

namespace XPlaneLauncher.Services.Impl {
    public class AircraftService : IAircraftService {
        private readonly IAircraftInformationDao _aircraftInformationDao;
        private readonly IAircraftModelRepository _aircraftModelRepository;
        private readonly ISitFileDao _sitFileDao;
        private readonly IThumbnailDao _thumbnailDao;
        private readonly ILauncherInformationDao _launcherInformationDao;

        public AircraftService(
            IAircraftInformationDao aircraftInformationDao, ILauncherInformationDao launcherInformationDao,
            IAircraftModelRepository aircraftModelRepository, ISitFileDao sitFileDao, IThumbnailDao thumbnailDao) {
            _aircraftInformationDao = aircraftInformationDao;
            _launcherInformationDao = launcherInformationDao;
            _aircraftModelRepository = aircraftModelRepository;
            _sitFileDao = sitFileDao;
            _thumbnailDao = thumbnailDao;
        }

        public async Task ReloadAsync() {
            _aircraftModelRepository.Aircrafts.Clear();
            IList<AircraftInformation> aircraftInformations = await _aircraftInformationDao.GetAllAsync();
            foreach (AircraftInformation aircraftInformation in aircraftInformations) {
                AircraftLauncherInformation aircraftLauncherInformation = await _launcherInformationDao.FindForAircraftAsync(aircraftInformation);
                Aircraft aircraft = new Aircraft(aircraftInformation, aircraftLauncherInformation);
                aircraft.Init();
                aircraft.Update(_sitFileDao.FindSit(aircraftInformation));
                aircraft.Update(_thumbnailDao.FindThumbnail(aircraftInformation));
                _aircraftModelRepository.Aircrafts.Add(aircraft);
            }
        }
    }
}