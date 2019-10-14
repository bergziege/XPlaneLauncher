using MapControl;

namespace XPlaneLauncher.Dtos {
    public class MapBoundary {
        public Location TopLeft { get; }
        public Location BottomRight { get; }

        public MapBoundary(Location topLeft, Location bottomRight) {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }
    }
}