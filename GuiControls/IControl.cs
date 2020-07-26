using Microsoft.Xna.Framework;
using Input;
using Microsoft.Xna.Framework.Content;

namespace GuiControls
{
    public interface IControl
    {
        Vector2 TopLeftPosition { get; }
        Vector2 TopRightPosition { get; }
        Vector2 BottomLeftPosition { get; }
        Vector2 BottomRightPosition { get; }

        Vector2 RelativePosition { get; }

        void LoadContent(ContentManager content);
        void Update(InputHandler input, float deltaTime, Matrix? transform = null);
        void Draw(Matrix? transform = null);
    }
}