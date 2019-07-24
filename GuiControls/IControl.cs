using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    public interface IControl
    {
        void Update(GameTime gameTime, Matrix? transform = null);
        void Draw(SpriteBatch spriteBatch = null, Matrix? transform = null);
    }
}