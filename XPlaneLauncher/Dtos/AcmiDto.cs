using System;
using System.Collections.Generic;
using MapControl;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Dtos {
    public class AcmiDto {
        public TimeSpan Duration { get; set; }
        public DateTime RecordingTime { get; set; }
        public double ReferenceLatitude { get; set; }
        public double ReferenceLongitude { get; set; }
        public DateTime ReferenceTime { get; set; }
        public IList<LogbookLocation> Track { get; set; } = new List<LogbookLocation>();
    }
}