using Microsoft.Xna.Framework;

namespace GuiControls
{
    public interface IControl
    {
        Vector2 Position { get; set; }

        void Update(GameTime gameTime, Matrix? transform = null);
        void Draw(Matrix? transform = null);
    }
}