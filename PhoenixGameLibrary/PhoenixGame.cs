using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        private readonly World _world;
        private readonly Cursor _cursor;

        public PhoenixGame()
        {
            _world = new World();
            _cursor = new Cursor();
        }

        public void LoadContent(ContentManager content)
        {
            _world.LoadContent(content);
            _cursor.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _world.Update(gameTime, input);
            _cursor.Update(gameTime);
        }

        public void Draw()
        {
            _world.Draw();
            _cursor.Draw();
        }
    }
}