using Input;
using Point = Utilities.Point;

namespace PhoenixGamePresentation
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