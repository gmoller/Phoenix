using Input;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    public class Cursor
    {
        public Point Position { get; private set; }

        public void Update(InputHandler input, float deltaTime)
        {
            Position = new Point(input.MousePosition.X, input.MousePosition.Y);
        }
    }
}