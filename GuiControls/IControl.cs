using Microsoft.Xna.Framework;
using Input;
using Microsoft.Xna.Framework.Content;

namespace GuiControls
{
    public interface IControl
    {
        int Top { get; }
        int Bottom { get; }
        int Left { get; }
        int Right { get; }
        Point Center { get; }
        Point TopLeft { get; }
        Point TopRight { get; }
        Point BottomLeft { get; }
        Point BottomRight { get; }
        int Width { get; }
        int Height { get; }
        Vector2 RelativePosition { get; }
        Point Size { get; }

        void SetTopLeftPosition(int x, int y);
        void LoadContent(ContentManager content);
        void Update(InputHandler input, float deltaTime, Matrix? transform = null);
        void Draw(Matrix? transform = null);
    }
}