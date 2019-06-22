using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utilities
{
    public sealed class DeviceManager
    {
        private static readonly Point Margin = new Point(10, 10);

        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        private ObjectPool<SpriteBatch> _spriteBatchesPool;
        private SpriteBatch _currentSpriteBatch;
        private Stack<Viewport> _viewports;

        public bool IsMouseVisible { get; set; } // TODO: remove this
        public GraphicsDevice GraphicsDevice { get; set; }

        public Viewport MapViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Y + Margin.Y, GraphicsDevice.Viewport.Width - Margin.X * 2, GraphicsDevice.Viewport.Height - Margin.Y * 2, 0, 1); 
        public Viewport MetricsViewport => new Viewport(GraphicsDevice.Viewport.X + Margin.X, GraphicsDevice.Viewport.Height - 200 - Margin.Y, GraphicsDevice.Viewport.Width - Margin.X * 2, 201, 0, 1);

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
            SpriteBatch spriteBatch;
            if (_spriteBatchesPool.HasFreeObject)
            {
                spriteBatch = _spriteBatchesPool.Get();
            }
            else
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);
            }

            return spriteBatch;
        }

        public void ReturnSpriteBatchToPool(SpriteBatch spriteBatch)
        {
            _spriteBatchesPool.Add(spriteBatch);
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