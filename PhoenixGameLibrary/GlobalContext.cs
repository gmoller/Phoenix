using Utilities;

namespace PhoenixGameLibrary
{
    public class GlobalContext
    {
        public Point DesiredResolution { get; set; }
        public Point ActualResolution { get; set; }
        public PointF ScreenRatio { get; set; }

        public Point WorldPositionPointedAtByMouseCursor { get; set; }
        public Point WorldHexPointedAtByMouseCursor { get; set; }

        public GameMetadata GameMetadata { get; set; }

        public object GraphicsDevice { get; set; }
        public object GraphicsDeviceManager { get; set; }
        public object GameWindow { get; set; }
    }
}