using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGamePresentation
{
    public class GlobalContextPresentation
    {
        public PointI DesiredResolution { get; set; }
        public PointI ActualResolution { get; set; }
        public Point2F ScreenRatio { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GameWindow GameWindow { get; set; }
        public Camera Camera { get; set; }
    }
}