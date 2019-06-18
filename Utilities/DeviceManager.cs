using Microsoft.Xna.Framework.Graphics;
using System;

namespace Utilities
{
    public sealed class DeviceManager
    {
        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        //public Rectangle Viewport { get; set; }

        //public Vector2 ScreenDimensions => new Vector2(Viewport.Width, Viewport.Height);
        //public int ScreenWidth => Viewport.Width;
        //public int ScreenHeight => Viewport.Height;
        public bool IsMouseVisible { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        private DeviceManager()
        {
        }
    }
}