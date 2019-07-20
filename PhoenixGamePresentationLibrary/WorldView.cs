using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    internal class WorldView
    {
        private readonly World _world;

        private readonly OverlandMapView _overlandMapView;
        private readonly SettlementView.SettlementView _settlementView;
        private readonly HudView _hudView;

        internal WorldView(World world)
        {
            _world = world;
            _overlandMapView = new OverlandMapView(world.OverlandMap);
            _settlementView = new SettlementView.SettlementView();
            _hudView = new HudView();
        }

        internal void LoadContent(ContentManager content)
        {
            _settlementView.LoadContent(content);
            _hudView.LoadContent(content);
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            _overlandMapView.Update(input);
            _hudView.Update(gameTime);

            if (_world.IsInSettlementView)
            {
                _settlementView.Settlement = _world.Settlement;
                _settlementView.Update(gameTime, input);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, _world.Camera.Transform);
            _overlandMapView.Draw(spriteBatch);
            spriteBatch.End();

            _hudView.Draw(spriteBatch);

            if (_world.IsInSettlementView)
            {
                _settlementView.Draw(spriteBatch);
            }
        }
    }
}