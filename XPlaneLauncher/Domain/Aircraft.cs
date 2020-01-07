using System;
using System.IO;

namespace XPlaneLauncher.Domain {
    public class Aircraft {
        private Guid _businessId;

        public Aircraft(AircraftInformation information, AircraftLauncherInformation launcherInformation) {
            Information = information;
            LauncherInformation = launcherInformation;
            _businessId = Guid.NewGuid();
        }

        public Guid BusinessId {
            get { return _businessId; }
        }

        public string FileName { get; set; }

        public FileInfo Thumbnail { get; set; }
        public bool HasSit { get; set; }
        public FileInfo SitFile { get; set; }

        public AircraftInformation Information { get; }
        public AircraftLauncherInformation LauncherInformation { get; }
    }
}