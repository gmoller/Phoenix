using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class World
    {
        private readonly OverlandMap _overlandMap;
        private readonly Settlements _settlements;

        public World()
        {
            _overlandMap = new OverlandMap();
            _settlements = new Settlements();
        }

        public void LoadContent(ContentManager content)
        {
            _overlandMap.LoadContent(content);
            _settlements.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _overlandMap.Update(gameTime, input);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _overlandMap.Draw(spriteBatch);
            _settlements.Draw(spriteBatch);
        }
    }
}