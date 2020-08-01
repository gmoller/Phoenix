using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utilities
{
    public sealed class DeviceManager
    {
        private static readonly Point Margin = new Point(0, 0);

        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        private readonly ObjectPool<SpriteBatch> _spriteBatchesPool;
        private SpriteBatch _currentSpriteBatch;
        private readonly Stack<Viewport> _viewports;

        public GraphicsDevice GraphicsDevice { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public Point WorldPositionPointedAtByMouseCursor { get; set; }
        public Point WorldHexPointedAtByMouseCursor { get; set; }

        public Viewport MapViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Y + Margin.Y, GraphicsDevice.Viewport.Width - Margin.X * 2, GraphicsDevice.Viewport.Height - Margin.Y * 2, 0, 1);
        //public Viewport MetricsViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Height - 200 - Margin.Y, GraphicsDevice.Viewport.Width - Margin.X * 2, 201, 0, 1);
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
            SizeRatio = new Vector2(width / 1920.0f, height / 1080.0f);
            //SizeRatio = new Vector2(width / 1000.0f, height / 1000.0f);
        }

        public Vector2 SizeRatio { get; private set; }

        private DeviceManager()
        {
            _viewports = new Stack<Viewport>();
            _spriteBatchesPool = new ObjectPool<SpriteBatch>();
        }

        public void SetCurrentSpriteBatch(SpriteBatch spriteBatch)
        {
            _currentSpriteBatch = spriteBatch;
        }

        public void DisposeSpriteBatches()
        {
            _currentSpriteBatch.Dispose();
            _spriteBatchesPool.Dispose();
        }

        public SpriteBatch GetCurrentSpriteBatch()
        {
            return _currentSpriteBatch;
        }

        public SpriteBatch GetNewSpriteBatch()
        {
            var spriteBatch = _spriteBatchesPool.HasFreeObject ? _spriteBatchesPool.Get() : new SpriteBatch(GraphicsDevice);
            //var spriteBatch = new SpriteBatch(GraphicsDevice);

            return spriteBatch;
        }

        public void ReturnSpriteBatchToPool(SpriteBatch spriteBatch)
        {
            _spriteBatchesPool.Add(spriteBatch);
            //spriteBatch.Dispose();
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