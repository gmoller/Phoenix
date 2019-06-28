using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using HexLibrary;
using PhoenixGameLibrary.Helpers;
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
        private const byte MAXIMUM_POPULATION_SIZE = 25;

        private readonly Camera _camera;
        private readonly CellGrid _cellGrid;
        private Texture2D _texture;
        private Rectangle _sourceRectangle;
        private Label _lblName;
        private SettlementView _settlementView;

        private int _populationGrowth;

        public string Name { get; }
        public Point Location { get; }
        public int Population => Citizens.TotalPopulation * 1000 + _populationGrowth; // every 1 citizen is 1000 population
        public int GrowthRate => DetermineGrowthRate();
        public SettlementCitizens Citizens { get; }

        public SettlementType SettlementType
        {
            get
            {
                if (Citizens.TotalPopulation <= 4) return SettlementType.Hamlet;
                if (Citizens.TotalPopulation >= 5 && Citizens.TotalPopulation <= 8) return SettlementType.Village;
                if (Citizens.TotalPopulation >= 9 && Citizens.TotalPopulation <= 12) return SettlementType.Town;
                if (Citizens.TotalPopulation >= 13 && Citizens.TotalPopulation <= 16) return SettlementType.City;
                if (Citizens.TotalPopulation >= 17) return SettlementType.Capital;

                throw new Exception("Unknown settlement type.");
            }
        }

        public Settlement(string name, Point location, byte settlementSize, CellGrid cellGrid, Camera camera)
        {
            _camera = camera;
            _cellGrid = cellGrid;
            Name = name;
            Location = location;
            Citizens = new SettlementCitizens(this, settlementSize);
            _populationGrowth = 0;

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
            _lblName.Click += lblNameClick;
            _lblName.Draw();

            spriteBatch.End();

            _settlementView.Draw();
        }

        public void EndTurn()
        {
            _populationGrowth += GrowthRate;
            if (_populationGrowth >= 1000 && Citizens.TotalPopulation < MAXIMUM_POPULATION_SIZE)
            {
                Citizens.IncreaseByOne();
                _populationGrowth = 0;
            }
        }

        private int DetermineGrowthRate()
        {
            int maxSettlementSize = DetermineMaximumSettlementSize();
            if (Citizens.TotalPopulation >= maxSettlementSize) return 0;

            float baseGrowthRate = (maxSettlementSize - Citizens.TotalPopulation + 1) / 2.0f;
            int baseGrowthRateRoundedUp = (int)Math.Ceiling(baseGrowthRate);

            int adjustedGrowthRate = baseGrowthRateRoundedUp * 10;

            return adjustedGrowthRate;
        }

        private int DetermineMaximumSettlementSize()
        {
            int baseFoodLevel = (int)BaseFoodLevel.DetermineBaseFoodLevel(Location, _cellGrid);

            return baseFoodLevel > MAXIMUM_POPULATION_SIZE ? MAXIMUM_POPULATION_SIZE : baseFoodLevel;
        }

        private void lblNameClick(object sender, EventArgs e)
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