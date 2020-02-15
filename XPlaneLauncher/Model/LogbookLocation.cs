using System;
using MapControl;

namespace XPlaneLauncher.Model {
    public class LogbookLocation : Location {
        public LogbookLocation(DateTime dateTime, double latitude, double longitude) : base(latitude, longitude) {
            DateTime = dateTime;
        }

        public double Alt { get; set; }
        public DateTime DateTime { get; set; }
        public double Hdg { get; set; }
        public double Ias { get; set; }
    }
}