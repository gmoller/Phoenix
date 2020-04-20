using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HexLibrary;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class UnitView
    {
        internal Unit Unit { get; set; }

        internal UnitView(Unit unit)
        {
            Unit = unit;
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(Unit.Location.X, Unit.Location.Y);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            var sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.Navy, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0.0f);
        }
    }
}