using System.Collections.Generic;
using System.IO;
using System.Security.RightsManagement;
using Newtonsoft.Json;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Properties;

namespace XPlaneLauncher.Services
{
    public class AircraftService
    {
        public IList<AircraftDto> GetAircrafts()
        {
            DirectoryInfo positionDataDir = new DirectoryInfo(Settings.Default.PositionFilesPath);
            DirectoryInfo sitFilePath = new DirectoryInfo(Settings.Default.SituationFilesPath);
            DirectoryInfo xplaneRootPath = new DirectoryInfo(Settings.Default.XPlaneRootPath);

            IList<AircraftDto> aircraftDtos = new List<AircraftDto>();
            foreach (FileInfo positionFile in positionDataDir.GetFiles("*.txt"))
            {
                string pureFileName = positionFile.Name.Replace(positionFile.Extension, "");
                AircraftDto dto = new AircraftDto();
                dto.FileName = positionFile.FullName;
                AircraftInformation info =
                    JsonConvert.DeserializeObject<AircraftInformation>(File.ReadAllText(positionFile.FullName));
                dto.Name = info.AircraftFile;
                dto.LiveryName = info.Livery;
                dto.Lat = info.Latitude;
                dto.Lon = info.Longitude;
                dto.Heading = info.Heading;

                FileInfo sitFile = new FileInfo($@"{sitFilePath.FullName}{Path.DirectorySeparatorChar}{pureFileName}.sit");
                if (sitFile.Exists)
                {
                    dto.HasSit = true;
                    dto.SitFile = sitFile;
                }

                FileInfo thumbnail = new FileInfo($@"{xplaneRootPath.FullName}{Path.DirectorySeparatorChar}{info.Livery}{info.AircraftFile?.Replace(".acf","")}{Settings.Default.ThumbnailEnding}");
                if (thumbnail.Exists)
                {
                    dto.Thumbnail = thumbnail;
                }
                aircraftDtos.Add(dto);
            }

            return aircraftDtos;
        }

        public AircraftLauncherInformation GetLauncherInformation(AircraftDto aircraft)
        {
            FileInfo aircraftFile = new FileInfo(aircraft.FileName);
            FileInfo launcherFile = new FileInfo(aircraftFile.FullName.Replace(aircraftFile.Extension, ".launcher"));
            if (launcherFile.Exists)
            {
                return JsonConvert.DeserializeObject<AircraftLauncherInformation>(
                    File.ReadAllText(launcherFile.FullName));
            }

            return null;
        }

        public void SaveLauncherInformation(AircraftDto aircraftDto, AircraftLauncherInformation info)
        {
            FileInfo aircraftFile = new FileInfo(aircraftDto.FileName);
            FileInfo launcherFile = new FileInfo(aircraftFile.FullName.Replace(aircraftFile.Extension, ".launcher"));
            File.WriteAllText(launcherFile.FullName, JsonConvert.SerializeObject(info));
        }
    }
}