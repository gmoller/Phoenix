using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utilities.ViewportAdapters
{
    public class DefaultViewportAdapter : ViewportAdapter
    {
        private readonly GraphicsDevice _graphicsDevice;

        protected override int VirtualWidth => _graphicsDevice.Viewport.Width;
        protected override int VirtualHeight => _graphicsDevice.Viewport.Height;
        protected override int ViewportWidth => _graphicsDevice.Viewport.Width;
        protected override int ViewportHeight => _graphicsDevice.Viewport.Height;

        public DefaultViewportAdapter(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public override Matrix GetScaleMatrix()
        {
            return Matrix.Identity;
        }
    }
}