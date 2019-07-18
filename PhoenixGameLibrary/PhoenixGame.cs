using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        private readonly World _world;

        public Cursor Cursor { get; }

        public PhoenixGame()
        {
            _world = new World();
            Cursor = new Cursor();
        }

        public void LoadContent(ContentManager content)
        {
            _world.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _world.Update(gameTime, input);
            Cursor.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void Draw()
        {
            _world.Draw();
        }
    }
}