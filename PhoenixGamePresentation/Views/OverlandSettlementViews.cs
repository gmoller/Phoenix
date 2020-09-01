using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    public class OverlandSettlementViews
    {
        #region State
        private readonly WorldView _worldView;
        private readonly Settlements _settlements;

        private readonly OverlandSettlementView _overlandSettlementView;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;
        #endregion

        public OverlandSettlementViews(WorldView worldView, Settlements settlements, InputHandler input)
        {
            _worldView = worldView;
            _settlements = settlements;

            _overlandSettlementView = new OverlandSettlementView(_worldView, input);

            SetupViewport(0, 0, 1670, 1080);
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

        public void Update(InputHandler input, float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            foreach (var settlement in _settlements)
            {
                _overlandSettlementView.Settlement = settlement;
                _overlandSettlementView.Update(input, deltaTime);
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
    }
}