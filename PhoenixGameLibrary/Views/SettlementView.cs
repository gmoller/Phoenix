using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary.Views
{
    public class SettlementView
    {
        private Texture2D _guiTextures;
        private AtlasSpec2 _guiAtlas;

        private MainFrame _mainFrame;
        private Label _lblSettlementName1;
        private Label _lblSettlementName2;
        private PopulationFrame _populationFrame;
        private ResourceFrame _resourceFrame;

        private readonly Settlement _settlement;
        private readonly Vector2 _topLeftPosition;

        public bool IsEnabled { get; set; }

        public SettlementView(Settlement settlement)
        {
            _settlement = settlement;
            IsEnabled = false;
            _topLeftPosition = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.65f, 200.0f);
        }

        public void LoadContent(ContentManager content)
        {
            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");

            _mainFrame = new MainFrame(this, _topLeftPosition, _guiTextures, _guiAtlas);

            var font = AssetsManager.Instance.GetSpriteFont("Carolingia-Regular-24");
            _lblSettlementName1 = new Label(font, new Vector2(_topLeftPosition.X + 278.0f, _topLeftPosition.Y - 49.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);
            _lblSettlementName2 = new Label(font, new Vector2(_topLeftPosition.X + 278.0f, _topLeftPosition.Y - 24.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);

            _populationFrame = new PopulationFrame(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 40.0f), _settlement, _guiTextures, _guiAtlas);
            _resourceFrame = new ResourceFrame(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 160.0f), _settlement, _guiTextures, _guiAtlas);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            if (IsEnabled)
            {
                _mainFrame.Update(gameTime, input);

                _lblSettlementName1.Text = $"{_settlement.SettlementType} of";
                _lblSettlementName2.Text = $"{_settlement.Name}";

                _populationFrame.Update(gameTime, input);
                _resourceFrame.Update(gameTime, input);
            }
        }

        public void Draw()
        {
            if (IsEnabled)
            {
                //DeviceManager.Instance.SetViewport(new Viewport(1035, 0, 556, 800, 0, 1));

                _mainFrame.Draw();

                _lblSettlementName1.Draw();
                _lblSettlementName2.Draw();

                _populationFrame.Draw();
                _resourceFrame.Draw();

                var catchment = HexOffsetCoordinates.GetAllNeighbors(_settlement.Location.X, _settlement.Location.Y);
                foreach (var tile in catchment)
                {
                    //var cell = Globals.Instance.CellGrid[tile.Col, tile.Row];
                    //cell.Draw(_camera, depth, _terrainTypes);
                }

                //DeviceManager.Instance.ResetViewport();
            }
        }

        public void CloseButtonClick(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }
    }

    public class MainFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;
        private readonly Texture2D _texture;
        private readonly Rectangle _main;
        private readonly Rectangle _heading;
        private readonly Rectangle _bottom;
        private readonly Button _btnClose;

        public MainFrame(SettlementView parent, Vector2 topLeftPosition, Texture2D texture, AtlasSpec2 atlas)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
            _texture = texture;

            var frame = atlas.Frames["frame_main"];
            _main = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_big_heading"];
            _heading = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_bottom"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            _btnClose = new Button(new Vector2(_topLeftPosition.X + 506.0f, _topLeftPosition.Y - 92.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(43.0f, 44.0f), "GUI_Textures_1", "close_button_n", "close_button_a", "close_button_h");
            _btnClose.Click += CloseButtonClick;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _btnClose.Update(gameTime);
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X, _topLeftPosition.Y), _main, Color.White);
            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X - 2.0f, _topLeftPosition.Y - 100.0f), _heading, Color.White);
            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X - 2.0f, _topLeftPosition.Y + 680.0f), _bottom, Color.White);

            spriteBatch.End();

            _btnClose.Draw();
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            _parent.CloseButtonClick(sender, e);
        }
    }

    public class PopulationFrame
    {
        private readonly Settlement _settlement;

        private readonly Label _lblRace;
        private readonly Label _lblPopulationGrowth;
        private readonly SmallFrame _smallFrame;
        private readonly Label _lblFarmers1;
        private readonly Label _lblFarmers2;
        private readonly Label _lblWorkers1;
        private readonly Label _lblWorkers2;

        public PopulationFrame(Vector2 topLeftPosition, Settlement settlement, Texture2D guiTextures, AtlasSpec2 guiAtlas)
        {
            _settlement = settlement;

            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            _lblRace = new Label(font, topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblPopulationGrowth = new Label(font, new Vector2(topLeftPosition.X + 516.0f, topLeftPosition.Y), HorizontalAlignment.Right, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);

            _smallFrame = new SmallFrame(topLeftPosition + new Vector2(0.0f, 10.0f), new Vector2(500.0f, 80.0f), 0, guiTextures, guiAtlas);
            _lblFarmers1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblFarmers2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblWorkers1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblWorkers2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblRace.Text = $"{_settlement.RaceType.Name}";
            _lblPopulationGrowth.Text = $"Population: {_settlement.Population} (+{_settlement.GrowthRate})";
            _smallFrame.Update(gameTime, input);
            _lblFarmers1.Text = "Farmers:";
            _lblFarmers2.Text = $"{_settlement.Citizens.SubsistenceFarmers + _settlement.Citizens.AdditionalFarmers}";
            _lblWorkers1.Text = "Workers:";
            _lblWorkers2.Text = $"{_settlement.Citizens.Workers}";
        }

        public void Draw()
        {
            _lblRace.Draw();
            _lblPopulationGrowth.Draw();
            _smallFrame.Draw();
            _lblFarmers1.Draw();
            _lblFarmers2.Draw();
            _lblWorkers1.Draw();
            _lblWorkers2.Draw();
        }
    }

    public class ResourceFrame
    {
        private readonly Settlement _settlement;

        private readonly Label _lblResources;
        private readonly SmallFrame _smallFrame;
        private readonly Label _lblFood1;
        private readonly Label _lblFood2;
        private readonly Label _lblProduction1;
        private readonly Label _lblProduction2;
        private readonly Label _lblGold1;
        private readonly Label _lblGold2;
        private readonly Label _lblPower1;
        private readonly Label _lblPower2;
        private readonly Label _lblResearch1;
        private readonly Label _lblResearch2;

        public ResourceFrame(Vector2 topLeftPosition, Settlement settlement, Texture2D guiTextures, AtlasSpec2 guiAtlas)
        {
            _settlement = settlement;

            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            _lblResources = new Label(font, topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _smallFrame = new SmallFrame(topLeftPosition + new Vector2(0.0f, 10.0f), new Vector2(500.0f, 160.0f), 0, guiTextures, guiAtlas);
            _lblFood1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblFood2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblProduction1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblProduction2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblGold1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 100.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblGold2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 100.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblPower1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 130.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblPower2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 130.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblResearch1 = new Label(font, new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 160.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblResearch2 = new Label(font, new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 160.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblResources.Text = "Resources";
            _lblFood1.Text = "Food";
            _lblFood2.Text = $"{_settlement.SettlementFoodProduction}";
            _lblProduction1.Text = "Production";
            _lblProduction2.Text = $"{_settlement.SettlementProduction}";
            _lblGold1.Text = "Gold";
            _lblGold2.Text = $"{0}";
            _lblPower1.Text = "Power";
            _lblPower2.Text = $"{0}";
            _lblResearch1.Text = "Research";
            _lblResearch2.Text = $"{0}";
        }

        public void Draw()
        {
            _lblResources.Draw();
            _smallFrame.Draw();
            _lblFood1.Draw();
            _lblFood2.Draw();
            _lblProduction1.Draw();
            _lblProduction2.Draw();
            _lblGold1.Draw();
            _lblGold2.Draw();
            _lblPower1.Draw();
            _lblPower2.Draw();
            _lblResearch1.Draw();
            _lblResearch2.Draw();
        }
    }

    public class SmallFrame
    {
        private readonly Vector2 _topLeftPosition;
        private readonly Vector2 _size;
        private readonly int _numberOfSlots;
        private readonly Texture2D _texture;
        private readonly Rectangle _top;
        private readonly Rectangle _left;
        private readonly Rectangle _right;
        private readonly Rectangle _bottom;
        private readonly Rectangle _corner;
        private readonly Rectangle _slot;

        public SmallFrame(Vector2 topLeftPosition, Vector2 size, int numberOfSlots, Texture2D texture, AtlasSpec2 atlas)
        {
            _topLeftPosition = topLeftPosition;
            _size = size;
            _numberOfSlots = numberOfSlots;
            _texture = texture;

            var frame = atlas.Frames["top_h_border_repeat_x"];
            _top = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["left_v_border_repeat_y"];
            _left = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["right_v_border_repeat_y"];
            _right = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["bottom_h_border_repeat_x"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_corner"];
            _corner = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["slot"];
            _slot = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            // frame
            var rectLeft = new Rectangle((int)_topLeftPosition.X, (int)_topLeftPosition.Y + 5, _left.Width, (int)_size.Y);
            spriteBatch.Draw(_texture, rectLeft, _left, Color.White);
            var rectRight = new Rectangle((int)(_topLeftPosition.X + _size.X + 0), (int)_topLeftPosition.Y + 5, _right.Width, (int)_size.Y);
            spriteBatch.Draw(_texture, rectRight, _right, Color.White);

            var rectTop = new Rectangle((int)_topLeftPosition.X + 6, (int)_topLeftPosition.Y, (int)_size.X, _top.Height);
            spriteBatch.Draw(_texture, rectTop, _top, Color.White);
            var rectBottom = new Rectangle((int)_topLeftPosition.X + 6, (int)(_topLeftPosition.Y + _size.Y), (int)_size.X, _bottom.Height);
            spriteBatch.Draw(_texture, rectBottom, _bottom, Color.White);

            // corners
            var rectTopLeft = new Rectangle((int)_topLeftPosition.X + 1, (int)_topLeftPosition.Y + 1, _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectTopLeft, _corner, Color.White);
            var rectTopRight = new Rectangle((int)(_topLeftPosition.X + _size.X - 2), (int)_topLeftPosition.Y + 1, _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectTopRight, _corner, Color.White);
            var rectBottomLeft = new Rectangle((int)(_topLeftPosition.X + _size.X - 2), (int)(_topLeftPosition.Y + _size.Y - 1), _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectBottomLeft, _corner, Color.White);
            var rectBottomRight = new Rectangle((int)_topLeftPosition.X + 1, (int)(_topLeftPosition.Y + _size.Y - 1), _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectBottomRight, _corner, Color.White);

            // slots
            float x = _topLeftPosition.X + 10.0f;
            float y = _topLeftPosition.Y + 10.0f;
            for (int i = 0; i < _numberOfSlots; ++i)
            {
                spriteBatch.Draw(_texture, new Vector2(x, y), _slot, Color.White);
                x += _slot.Width + 0.0f;
            }

            spriteBatch.End();
        }
    }
}