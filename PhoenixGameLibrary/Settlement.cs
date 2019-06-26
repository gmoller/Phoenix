using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A settlement is an immovable game entity that can be controlled by the
    /// player/AI to do things such as build new units and add buildings to
    /// improve the settlement.
    /// </summary>
    public class Settlement
    {
        private readonly Camera _camera;
        private Texture2D _texture;
        private Rectangle _sourceRectangle;
        private Label _lblName;
        private CityView _cityView;

        public string Name { get; }
        public Point Location { get; }
        public int Population { get; private set; }

        public SettlementType SettlementType
        {
            get
            {
                if (Population <= 4) return SettlementType.Hamlet;
                if (Population >= 5 && Population <= 8) return SettlementType.Village;
                if (Population >= 9 && Population <= 12) return SettlementType.Town;
                if (Population >= 13 && Population <= 16) return SettlementType.City;
                if (Population >= 17)  return SettlementType.Capital;

                throw new Exception("Unknown settlement type.");
            }
        }

        public Settlement(string name, Point location, int settlementSize, Camera camera)
        {
            _camera = camera;
            Name = name;
            Location = location;
            Population = settlementSize * 1000;

            _lblName = new Label(null, Vector2.Zero, HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, Name, HorizontalAlignment.Center, Color.Cyan, null, Color.Black * 0.5f);
            _cityView = new CityView();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            _cityView.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblName.Update(gameTime);
            _cityView.Update(gameTime, input);
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);

            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(Location.X, Location.Y);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 111, 192);
            spriteBatch.Draw(_texture, destinationRectangle, _sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, 0.0f);

            var font = AssetsManager.Instance.GetSpriteFont("Carolingia-Regular-36");
            var size = font.MeasureString(Name);
            position -= new Vector2(0.0f, HexLibrary.Constants.HEX_THREE_QUARTER_HEIGHT);
            _lblName = new Label(font, position, HorizontalAlignment.Center, VerticalAlignment.Middle, size, Name, HorizontalAlignment.Center, Color.Cyan, null, Color.Black * 0.5f, null, _camera.Transform);
            _lblName.Click += labelClick;
            _lblName.Draw();

            spriteBatch.End();

            _cityView.Draw();
        }

        private void labelClick(object sender, EventArgs e)
        {
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(Location.X, Location.Y);
            _camera.LookAt(position);
            _cityView.IsEnabled = true;
        }
    }

    public enum SettlementType
    {
        Outpost,
        Hamlet,
        Village,
        Town,
        City,
        Capital
    }
}