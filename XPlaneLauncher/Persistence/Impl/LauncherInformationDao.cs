using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence.Impl {
    public class LauncherInformationDao : ILauncherInformationDao {
        public async Task<AircraftLauncherInformation> FindForAircraftAsync(AircraftInformation aircraftInformation) {
            FileInfo aircraftFile = aircraftInformation.File;
            FileInfo launcherFile = new FileInfo(aircraftFile.FullName.Replace(aircraftFile.Extension, ".launcherV2"));
            if (launcherFile.Exists) {
                string launcherContent;
                using (StreamReader reader = File.OpenText(launcherFile.FullName)) {
                    launcherContent = await reader.ReadToEndAsync();
                }

                return JsonConvert.DeserializeObject<AircraftLauncherInformation>(launcherContent);
            }

            return null;
        }

        //public AircraftLauncherInformation GetLauncherInformation(AircraftDto aircraft) {
        //    
        //}

        //public void SaveLauncherInformation(AircraftDto aircraftDto, AircraftLauncherInformation info) {
        //    FileInfo aircraftFile = new FileInfo(aircraftDto.File);
        //    FileInfo launcherFile = new FileInfo(aircraftFile.FullName.Replace(aircraftFile.Extension, ".launcherV2"));
        //    File.WriteAllText(launcherFile.FullName, JsonConvert.SerializeObject(info));
        //}
    }
}