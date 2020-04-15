using Microsoft.Xna.Framework;
using Input;

namespace PhoenixGamePresentationLibrary
{
    public class Cursor
    {
        public Vector2 Position { get; private set; }

        public void Update(float deltaTime)
        {
            Position = new Vector2(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y);
        }
    }
}