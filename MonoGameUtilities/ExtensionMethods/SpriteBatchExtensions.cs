using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class SpriteBatchExtensions
    {
        public static void Draw_(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            var sourceRectangle = texture.Bounds;
            var origin = sourceRectangle.Size.ToVector2() * 0.5f;

            spriteBatch.Draw_(texture, destinationRectangle, sourceRectangle, color, origin);
        }

        public static void Draw_(this SpriteBatch spriteBatch, Texture2D texture2D, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, Vector2 origin)
        {
            spriteBatch.Draw(texture2D, destinationRectangle, sourceRectangle, color, 0.0f, origin, SpriteEffects.None, 0.0f);
        }
    }
}