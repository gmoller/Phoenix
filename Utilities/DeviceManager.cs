using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities.ViewportAdapters;

namespace Utilities
{
    public sealed class DeviceManager
    {
        private static readonly Point Margin = new Point(0, 0);

        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        private SpriteBatch _currentSpriteBatch;
        private readonly Stack<Viewport> _viewports;

        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GameWindow Window { get; set; }
        public ViewportAdapter ViewportAdapter { get; set; }
        public Point WorldPositionPointedAtByMouseCursor { get; set; }
        public Point WorldHexPointedAtByMouseCursor { get; set; }

        public Viewport MetricsViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Y + Margin.Y, 300, 201, 0, 1);

        public Point ScreenResolution { get; private set; }

        public void SetScreenResolution(int width, int height)
        {
            if (GraphicsDeviceManager != null)
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = width;
                GraphicsDeviceManager.PreferredBackBufferHeight = height;
                //GraphicsDeviceManager.IsFullScreen = true;
                GraphicsDeviceManager.ApplyChanges();
            }

            ScreenResolution = new Point(width, height);
        }

        private DeviceManager()
        {
            _viewports = new Stack<Viewport>();
        }

        public void SetCurrentSpriteBatch(SpriteBatch spriteBatch)
        {
            _currentSpriteBatch = spriteBatch;
        }

        public void DisposeSpriteBatches()
        {
            _currentSpriteBatch.Dispose();
        }

        public SpriteBatch GetCurrentSpriteBatch()
        {
            return _currentSpriteBatch;
        }

        public void SetViewport(Viewport newViewport)
        {
            _viewports.Push(GraphicsDevice.Viewport);
            GraphicsDevice.Viewport = newViewport;
        }

        public void ResetViewport()
        {
            var previousViewport = _viewports.Pop();
            GraphicsDevice.Viewport = previousViewport;
        }
    }
}