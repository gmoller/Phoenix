using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Utilities
{
    public sealed class DeviceManager
    {
        private static readonly Point Margin = new Point(10, 10);

        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        public bool IsMouseVisible { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public Viewport MapViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Y + Margin.Y, GraphicsDevice.Viewport.Width - Margin.X * 2, GraphicsDevice.Viewport.Height - Margin.Y, 0, 1); 
        public Viewport MetricsViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Height - 200 - Margin.Y, GraphicsDevice.Viewport.Width - Margin.X * 2, 201, 0, 1);

        private DeviceManager()
        {
        }
    }
}