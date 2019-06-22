using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Settlements
    {
        private readonly Camera _camera;
        private Texture2D _texture;

        private Rectangle _sourceRectangle;
        private List<Settlement> _settlements;

        public Settlements(Camera camera)
        {
            _camera = camera;
            _settlements = new List<Settlement>();
            _settlements.Add(new Settlement("Fairhaven", new Point(6, 6), 4));
        }

        public void LoadContent(ContentManager content)
        {
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw()
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MapViewport);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);

            foreach (var settlement in _settlements)
            {
                var position = World.CalculateWorldPosition(settlement.Location.X, settlement.Location.Y, _camera);
                spriteBatch.Draw(_texture, position, _sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, 0.0f);

                var font = AssetsManager.Instance.GetSpriteFont("Carolingia-Regular-36");
                var size = font.MeasureString(settlement.Name);
                position -= new Vector2(0.0f, Constants.HEX_THREE_QUARTER_HEIGHT);
                var label = new Label(font, position, HorizontalAlignment.Center, VerticalAlignment.Middle, size, settlement.Name, HorizontalAlignment.Center, Color.Cyan, null, Color.Black * 0.5f);
                label.Draw(_camera.Transform);
            }

            spriteBatch.End();

            DeviceManager.Instance.ResetViewport();
        }
    }
}