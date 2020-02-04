using System.Collections.Generic;
using System.IO;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using MapControl;
using Newtonsoft.Json;

namespace XPlaneLauncher.Persistence.Impl {
    public class LogbookEntryTrackDao : ILogbookEntryTrackDao {
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