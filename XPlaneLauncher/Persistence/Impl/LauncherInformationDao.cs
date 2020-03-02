using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence.Impl {
    public class LauncherInformationDao : ILauncherInformationDao {
        public void Delete(FileInfo aircraftLauncherInfoFile) {
            if (aircraftLauncherInfoFile != null && aircraftLauncherInfoFile.Exists) {
                aircraftLauncherInfoFile.Delete();
            }
        }

        public async Task<AircraftLauncherInformation> FindOrCreateForAircraftAsync(AircraftInformation aircraftInformation) {
            FileInfo aircraftFile = aircraftInformation.File;
            FileInfo launcherFile = new FileInfo(aircraftFile.FullName.Replace(aircraftFile.Extension, ".launcherV2"));
            if (!launcherFile.Exists) {
                CreateLauncherFile(launcherFile);
            }

            if (launcherFile.Exists) {
                string launcherContent;
                using (StreamReader reader = File.OpenText(launcherFile.FullName)) {
                    launcherContent = await reader.ReadToEndAsync();
                }

                AircraftLauncherInformation aircraftLauncherInformation = JsonConvert.DeserializeObject<AircraftLauncherInformation>(launcherContent);
                aircraftLauncherInformation.LauncherInfoFile = launcherFile;
                return aircraftLauncherInformation;
            }

            return null;
        }

        public void SaveToFile(FileInfo launcherInfoFile, AircraftLauncherInformation launcherInfo) {
            File.WriteAllText(launcherInfoFile.FullName, JsonConvert.SerializeObject(launcherInfo));
        }

        private void CreateLauncherFile(FileInfo launcherFile) {
            SaveToFile(launcherFile, new AircraftLauncherInformation());
        }
    }
}