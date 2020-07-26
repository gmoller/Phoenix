using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class HudView
    {
        private readonly WorldView _worldView;

        private readonly Rectangle _area;

        private IControl _resourceFrame;

        private Image _imgGold;
        private Image _imgMana;
        private Image _imgFood;

        private SpriteFont _font;
        private Frame _frame;

        private Label _lblCurrentDate;
        private Label _lblGold;
        private Label _lblMana;
        private Label _lblFood;

        private Button _btnEndTurn;

        private readonly UnitsView _unitsView;

        internal HudView(WorldView worldView, UnitsView unitsView)
        {
            var width = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.1305f); // 13.05% of screen width
            var height = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Height * 0.945f); // 94.5% of screen height
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 250x1020

            _worldView = worldView;
            _unitsView = unitsView;
        }

        internal void LoadContent(ContentManager content)
        {
            var topLeftPosition = new Vector2(_area.X, _area.Y);
            _resourceFrame = new Frame("ResourceFrame", topLeftPosition + new Vector2(10.0f, 50.0f), new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole", 0, 0, 0, 0, null);
            _resourceFrame.LoadContent(content);

            _imgGold = new Image("imgGold", new Vector2(10.0f, 10.0f), new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R", 0.0f, _resourceFrame);
            _imgGold.LoadContent(content);
            _imgMana = new Image("imgMana", _imgGold.RelativePosition + new Vector2(0.0f, _imgGold.Height) + new Vector2(0.0f, 10.0f), new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R", 0.0f, _resourceFrame);
            _imgMana.LoadContent(content);
            _imgFood = new Image("imgFood", _imgMana.RelativePosition + new Vector2(0.0f, _imgMana.Height) + new Vector2(0.0f, 10.0f), new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R", 0.0f, _resourceFrame);
            _imgFood.LoadContent(content);
            _lblGold = new Label("lblGold", "CrimsonText-Regular-12", _imgGold.RelativePosition + new Vector2(_imgGold.Width, 0.0f) + new Vector2(20.0f, 0.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack, null, null, _resourceFrame);
            _lblGold.LoadContent(content);
            _lblMana = new Label("lblMana", "CrimsonText-Regular-12", new Vector2(_imgMana.Right + 20.0f, _imgMana.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack, null, null, _resourceFrame);
            _lblMana.LoadContent(content);
            _lblFood = new Label("lblFood", "CrimsonText-Regular-12", new Vector2(_imgFood.Right + 20.0f, _imgFood.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack, null, null, _resourceFrame);
            _lblFood.LoadContent(content);

            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            var size = new Vector2(_area.Width, _area.Height);
            _frame = new Frame("Frame", topLeftPosition, size, "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);
            _frame.LoadContent(content);

            var topCenterPosition = new Vector2(topLeftPosition.X + size.X / 2.0f, topLeftPosition.Y) + new Vector2(0.0f, 10.0f);
            _lblCurrentDate = new Label("lblCurrentDate", "Maleficio-Regular-18", topCenterPosition, HorizontalAlignment.Center, VerticalAlignment.Top, new Vector2(10,10), "Date:", HorizontalAlignment.Center, Color.Aquamarine);

            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var label = new Label("lblNextTurn", "CrimsonText-Regular-12", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue);
            _btnEndTurn = new Button("btnEndTurn", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_a", "reg_button_h", label);
            _btnEndTurn.Click += BtnEndTurnClick;
        }

        public void Update(InputHandler input, float deltaTime)
        {
            var mouseOver = _area.Contains(input.MousePosition);
            input.Eaten = mouseOver;

            _resourceFrame.Update(input, deltaTime);

            _lblCurrentDate.Update(input, deltaTime);
            _lblCurrentDate.Text = Globals.Instance.World.CurrentDate;

            _imgGold.Update(input, deltaTime);
            _lblGold.Update(input, deltaTime);
            _lblGold.Text = $"{Globals.Instance.World.PlayerFaction.GoldInTreasury} GP (+{Globals.Instance.World.PlayerFaction.GoldPerTurn})";

            _imgMana.Update(input, deltaTime);
            _lblMana.Update(input, deltaTime);
            _lblMana.Text = "5 MP (+1)";

            _imgFood.Update(input, deltaTime);
            _lblFood.Update(input, deltaTime);
            _lblFood.Text = $"{Globals.Instance.World.PlayerFaction.FoodPerTurn} Food";

            _btnEndTurn.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _frame.Draw();
            spriteBatch.End();

            _resourceFrame.Draw();

            _lblCurrentDate.Draw();

            _imgGold.Draw();
            _lblGold.Draw();

            _imgMana.Draw();
            _lblMana.Draw();

            _imgFood.Draw();
            _lblFood.Draw();

            spriteBatch.Begin();
            DrawNotifications(spriteBatch);
            DrawSelectedUnits(spriteBatch);
            DrawTileInfo(spriteBatch);
            spriteBatch.End();

            _btnEndTurn.Draw();
        }

        private void DrawNotifications(SpriteBatch spriteBatch)
        {
            var x = _area.X + 10.0f;
            var y = _area.Y + 400.0f;
            foreach (var item in Globals.Instance.World.NotificationList)
            {
                var lines = TextWrapper.WrapText(item, 150, _font);
                foreach (var line in lines)
                {
                    spriteBatch.DrawString(_font, line, new Vector2(x, y), Color.Pink);
                    y += 20.0f;
                }
            }
        }

        private void DrawSelectedUnits(SpriteBatch spriteBatch)
        {
            var startX = _area.X + 10.0f;
            var startY = _area.Y + _area.Height / 2.0f;
            var x = startX;
            var y = startY;

            foreach (var unitView in _unitsView)
            {
                if (unitView.IsSelected)
                {
                    unitView.DrawBadge(spriteBatch, new Vector2(x, y));
                    x += 75.0f;
                    if (x > startX + 150.0f)
                    {
                        x = startX;
                        y += 75.0f;
                    }
                }
            }
        }

        private void DrawTileInfo(SpriteBatch spriteBatch)
        {
            var x = _area.X + 10.0f;
            var y = _area.Y + _area.Height * 0.96f;

            // get tile mouse is over
            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            var hexPoint = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            if (hexPoint.X >= 0 && hexPoint.Y >= 0 && hexPoint.X < PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS && hexPoint.Y < PhoenixGameLibrary.Constants.WORLD_MAP_ROWS)
            {
                var cell = cellGrid.GetCell(hexPoint.X, hexPoint.Y);
                if (cell.SeenState != SeenState.Never)
                {
                    var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
                    var text1 = $"{terrainType.Name} - {terrainType.FoodOutput} food";
                    spriteBatch.DrawString(_font, text1, new Vector2(x, y), Color.White);

                    if (terrainType.CanSettleOn)
                    {
                        var catchment = cellGrid.GetCatchment(hexPoint.X, hexPoint.Y, 2);
                        var maxPop = PhoenixGameLibrary.Helpers.BaseFoodLevel.DetermineBaseFoodLevel(new Utilities.Point(hexPoint.X, hexPoint.Y), catchment);
                        var text2 = $"Maximum Pop - {maxPop}";
                        spriteBatch.DrawString(_font, text2, new Vector2(x, y + 15.0f), Color.White);
                    }
                }
            }
        }

        private void BtnEndTurnClick(object sender, EventArgs e)
        {
            _worldView.EndTurn();
        }
    }
}