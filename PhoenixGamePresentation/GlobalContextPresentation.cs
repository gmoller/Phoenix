using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace PhoenixGamePresentation
{
    public class GlobalContextPresentation
    {
        public PointI DesiredResolution { get; set; }
        public PointI ActualResolution { get; set; }
        public PointF ScreenRatio { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GameWindow GameWindow { get; set; }

        public PointI WorldPositionPointedAtByMouseCursor { get; set; }
        public PointI WorldHexPointedAtByMouseCursor { get; set; }
    }
}