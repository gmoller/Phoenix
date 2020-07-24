using Microsoft.Xna.Framework;
using Input;

namespace PhoenixGamePresentationLibrary
{
    public class Cursor
    {
        public Vector2 Position { get; private set; }

        public void Update(InputHandler input, float deltaTime)
        {
            Position = new Vector2(input.MousePosition.X, input.MousePosition.Y);
        }
    }
}