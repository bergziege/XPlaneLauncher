using XPlaneLauncher.Ui.Modules.Map.Dtos;

namespace XPlaneLauncher.Ui.Modules.Map.Events {
    internal class MapBoundariesChangedEvent {
        public MapBoundary MapBoundary { get; }

        public MapBoundariesChangedEvent(MapBoundary mapBoundary) {
            MapBoundary = mapBoundary;
        }
    }
}