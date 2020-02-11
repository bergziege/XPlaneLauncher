using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using MapControl;
using Newtonsoft.Json;

namespace XPlaneLauncher.Persistence.Impl {
    public class LogbookEntryTrackDao : ILogbookEntryTrackDao {
        public void Delete(FileInfo trackFile) {
            if (trackFile.Exists) {
                trackFile.Delete();
            }
        }

        public IList<Location> GetTrack(FileInfo trackFile) {
            string trackFileContent = File.ReadAllText(trackFile.FullName);
            FeatureCollection points = JsonConvert.DeserializeObject<FeatureCollection>(trackFileContent);
            IList<Location> track = new List<Location>();
            foreach (Point point in points.Features.Select(x => x.Geometry as Point)) {
                track.Add(new Location(point.Coordinates.Latitude, point.Coordinates.Longitude));
            }

            return track;
        }

        public void Save(FileInfo trackFile, IList<Location> track) {
            /* Using Feature and FeatureCollection to be able to ad additional information to each location like altitude, heading, airspeed, ... */
            FeatureCollection features = new FeatureCollection();
            foreach (Location location in track) {
                Position position = new Position(location.Latitude, location.Longitude);
                Point point = new Point(position);
                features.Features.Add(new Feature(point));
            }

            string geoJson = JsonConvert.SerializeObject(features);
            File.WriteAllText(trackFile.FullName, geoJson);
        }
    }
}