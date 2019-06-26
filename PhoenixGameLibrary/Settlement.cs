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
        private SettlementView _settlementView;

        public string Name { get; }
        public Point Location { get; }
        public int Population { get; private set; } // every 1 citizen is 1000 population
        public short PopulationGrowth => 40; // TODO: implement growth
        public byte Size { get; private set; }
        public SettlementCitizens Citizens { get; }

        public SettlementType SettlementType
        {
            get
            {
                if (Size <= 4) return SettlementType.Hamlet;
                if (Size >= 5 && Size <= 8) return SettlementType.Village;
                if (Size >= 9 && Size <= 12) return SettlementType.Town;
                if (Size >= 13 && Size <= 16) return SettlementType.City;
                if (Size >= 17) return SettlementType.Capital;

                throw new Exception("Unknown settlement type.");
            }
        }

        public Settlement(string name, Point location, byte settlementSize, Camera camera)
        {
            _camera = camera;
            Name = name;
            Location = location;
            Population = settlementSize * 1000;
            Size = settlementSize;
            Citizens = new SettlementCitizens(this);

            _lblName = new Label(null, Vector2.Zero, HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, Name, HorizontalAlignment.Center, Color.Cyan, null, Color.Black * 0.5f);
            _settlementView = new SettlementView(this);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            _settlementView.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblName.Update(gameTime);
            _settlementView.Update(gameTime, input);
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

            _settlementView.Draw();
        }

        private void labelClick(object sender, EventArgs e)
        {
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(Location.X, Location.Y);
            _camera.LookAt(position);
            _settlementView.IsEnabled = true;
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