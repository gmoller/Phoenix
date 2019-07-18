using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    internal class WorldView
    {
        private World _world;

        private OverlandMapView _overlandMap;
        private HudView _hud;

        internal WorldView(World world)
        {
            _world = world;
            _overlandMap = new OverlandMapView(world.OverlandMap);
            _hud = new HudView();
        }

        internal void LoadContent(ContentManager content)
        {
            _hud.LoadContent(content);
        }

        internal void Update(GameTime gameTime)
        {
            _hud.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, _world.Camera.Transform);

            //_overlandMap.Draw(spriteBatch);

            spriteBatch.End();

            _hud.Draw(spriteBatch);
        }
    }
}