using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    public class OverlandSettlementViews : IDisposable
    {
        #region State
        private readonly WorldView _worldView;
        private readonly Settlements _settlements;

        private readonly OverlandSettlementView _overlandSettlementView;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion

        public OverlandSettlementViews(WorldView worldView, Settlements settlements, InputHandler input)
        {
            _worldView = worldView;
            _settlements = settlements;

            _overlandSettlementView = new OverlandSettlementView(_worldView, input);

            SetupViewport(0, 0, 1670, 1080);

            _input = input;
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            _viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            _viewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        public void LoadContent(ContentManager content)
        {
            _overlandSettlementView.LoadContent(content);
        }

        public void Update(float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            foreach (var settlement in _settlements)
            {
                _overlandSettlementView.Settlement = settlement;
                _overlandSettlementView.Update(deltaTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform * _viewportAdapter.GetScaleMatrix()); // FrontToBack

            foreach (var settlement in _settlements)
            {
                _overlandSettlementView.Settlement = settlement;
                _overlandSettlementView.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)
                _overlandSettlementView.Dispose();

                // TODO: set large fields to null
                _viewportAdapter = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}