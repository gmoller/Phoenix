using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentation
{
    public class GlobalContextPresentation
    {
        public Point DesiredResolution { get; set; }
        public Point ActualResolution { get; set; }
        public PointF ScreenRatio { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GameWindow GameWindow { get; set; }

        public Point WorldPositionPointedAtByMouseCursor { get; set; }
        public Point WorldHexPointedAtByMouseCursor { get; set; }
    }
}