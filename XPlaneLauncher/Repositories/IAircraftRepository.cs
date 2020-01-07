using System.Collections.Generic;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Repositories {
    public interface IAircraftRepository {
        IList<Aircraft> Aircrafts { get; }
    }
}