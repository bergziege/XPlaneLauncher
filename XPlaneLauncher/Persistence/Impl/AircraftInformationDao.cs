using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence.Impl {
    public class AircraftInformationDao : IAircraftInformationDao {
        public async Task<IList<AircraftInformation>> GetAllAsync() {
            DirectoryInfo positionDataDir = new DirectoryInfo(Properties.Settings.Default.DataPath);

            IList<AircraftInformation> aircraftDtos = new List<AircraftInformation>();
            foreach (FileInfo positionFile in positionDataDir.GetFiles("*.txt")) {
                AircraftInformation aircraftInformation = new AircraftInformation();
                aircraftInformation.File = positionFile;
                string aircaftInformationContent;
                using (StreamReader reader = File.OpenText(positionFile.FullName)) {
                    aircaftInformationContent = await reader.ReadToEndAsync();
                }

                AircraftInformation info = JsonConvert.DeserializeObject<AircraftInformation>(aircaftInformationContent);
                aircraftInformation.AircraftFile = info.AircraftFile;
                aircraftInformation.Livery = info.Livery;
                aircraftInformation.Latitude = info.Latitude;
                aircraftInformation.Longitude = info.Longitude;
                aircraftInformation.Heading = info.Heading;

                aircraftDtos.Add(aircraftInformation);
            }

            return aircraftDtos;
        }

        public void Delete(FileInfo aircraftInformationFile) {
            if (aircraftInformationFile != null && aircraftInformationFile.Exists) {
                aircraftInformationFile.Delete();
            }
        }
    }
}