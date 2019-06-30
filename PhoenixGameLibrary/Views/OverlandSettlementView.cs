using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using HexLibrary;
using Utilities;
using System;

namespace PhoenixGameLibrary.Views
{
    public class OverlandSettlementView
    {
        private Settlement _settlement;

        private Label _lblName;
        private Texture2D _texture;
        private Rectangle _sourceRectangle;

        public OverlandSettlementView(Settlement settlement)
        {
            _settlement = settlement;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

            _lblName = new Label("lblName", "Carolingia-Regular-36", Vector2.Zero, HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, _settlement.Name, HorizontalAlignment.Center, Color.Cyan, null, Color.Black * 0.5f);
            _lblName.Click += lblNameClick;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblName.Update(gameTime);
        }

        public void Draw()
        {
            var camera = Globals.Instance.World.Camera;
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, camera.Transform);

            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(_settlement.Location.X, _settlement.Location.Y);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(HexLibrary.Constants.HEX_ACTUAL_WIDTH * 0.5f), (int)(HexLibrary.Constants.HEX_ACTUAL_HEIGHT * 0.75f));
            spriteBatch.Draw(_texture, destinationRectangle, _sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, 0.0f);

            position -= new Vector2(0.0f, HexLibrary.Constants.HEX_THREE_QUARTER_HEIGHT);
            _lblName.Position = position;
            _lblName.Transform = Globals.Instance.World.Camera.Transform;
            _lblName.Draw();

            spriteBatch.End();
        }

        private void lblNameClick(object sender, EventArgs e)
        {
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(_settlement.Location.X, _settlement.Location.Y);
            Globals.Instance.World.Camera.LookAt(position);
            _settlement.View.IsEnabled = true;
        }
    }
}