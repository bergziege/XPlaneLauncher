using System;
using CsvHelper.Configuration.Attributes;

namespace XPlaneLauncher.Domain {
    public class LogbookTrackItem {
        [Index(3)]
        [Name("alt")]
        public double Alt { get; set; }

        [Index(4)]
        [Name("hdg")]
        public double Hdg { get; set; }

        [Index(5)]
        [Name("ias")]
        public double Ias { get; set; }

        [Index(1)]
        [Name("lat")]
        public double Lat { get; set; }

        [Index(2)]
        [Name("lon")]
        public double Lon { get; set; }

        [Index(0)]
        [Name("time")]
        public DateTime Time { get; set; }
    }
}