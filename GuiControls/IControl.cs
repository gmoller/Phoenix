using Microsoft.Xna.Framework;
using Input;

namespace GuiControls
{
    public interface IControl
    {
        Vector2 Position { get; set; }

        void Update(InputHandler input, float deltaTime, Matrix? transform = null);
        void Draw(Matrix? transform = null);
    }
}