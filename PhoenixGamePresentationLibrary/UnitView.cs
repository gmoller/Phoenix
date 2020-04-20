using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HexLibrary;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class UnitView
    {
        private readonly WorldView _worldView;

        internal Unit Unit { get; set; }

        internal UnitView(WorldView worldView, Unit unit)
        {
            _worldView = worldView;
            Unit = unit;
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            if (input.IsRightMouseButtonReleased)
            {
                var hex = DeviceManager.Instance.WorldHex;
                var hexPoint = new Utilities.Point(hex.X, hex.Y);
                if (Unit.Location == hexPoint)
                {
                    // TODO: make unit flash, set as selected unit, show in hudview

                    var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(hex.X, hex.Y);
                    _worldView.Camera.LookAt(worldPixelLocation);
                }
            }
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