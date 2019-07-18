using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }

        public Cursor Cursor { get; }

        public PhoenixGame()
        {
            World = new World();
            Cursor = new Cursor();
        }

        public void LoadContent(ContentManager content)
        {
            World.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            World.Update(gameTime, input);
            Cursor.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            World.Draw(spriteBatch);
        }
    }
}