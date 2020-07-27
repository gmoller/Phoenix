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

        private Label2 _lblCurrentDate;
        private Label2 _lblGold;
        private Label2 _lblMana;
        private Label2 _lblFood;

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
            _resourceFrame = new Frame("ResourceFrame", topLeftPosition + new Vector2(10.0f, 50.0f), ContentAlignment.TopLeft, new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole", null, 0, 0, 0, 0, null);
            _resourceFrame.LoadContent(content);

            _imgGold = new Image("imgGold", new Vector2(10.0f, 10.0f), new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R", 0.0f, _resourceFrame);
            _imgGold.LoadContent(content);
            _imgMana = new Image("imgMana", _imgGold.RelativePosition + new Vector2(0.0f, _imgGold.Height) + new Vector2(0.0f, 10.0f), new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R", 0.0f, _resourceFrame);
            _imgMana.LoadContent(content);
            _imgFood = new Image("imgFood", _imgMana.RelativePosition + new Vector2(0.0f, _imgMana.Height) + new Vector2(0.0f, 10.0f), new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R", 0.0f, _resourceFrame);
            _imgFood.LoadContent(content);
            _lblGold = new Label2("lblGold", new Vector2(80.0f, 25.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 20.0f), "", "CrimsonText-Regular-12", Color.Yellow, null, 0.0f, _resourceFrame); //_imgGold.TopRightRelative.ToVector2()
            _lblGold.LoadContent(content);
            _lblMana = new Label2("lblMana", new Vector2(80.0f, 85.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 20.0f), "", "CrimsonText-Regular-12", Color.Yellow, null, 0.0f, _resourceFrame); //_imgMana.TopRightRelative.ToVector2()
            _lblMana.LoadContent(content);
            _lblFood = new Label2("lblFood", new Vector2(80.0f, 145.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 20.0f), "", "CrimsonText-Regular-12", Color.Yellow, null, 0.0f, _resourceFrame); //_imgFood.TopRightRelative.ToVector2()
            _lblFood.LoadContent(content);

            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            var size = new Vector2(_area.Width, _area.Height);
            _frame = new Frame("Frame", topLeftPosition, ContentAlignment.TopLeft, size, "GUI_Textures_1", "frame3_whole", null, 47, 47, 47, 47);
            _frame.LoadContent(content);

            _lblCurrentDate = new Label2("lblCurrentDate", new Vector2(_frame.Width * 0.5f,20.0f), ContentAlignment.MiddleCenter, new Vector2(130, 23), "Date:", "Maleficio-Regular-18", Color.Aquamarine, null, 0.0f, _frame);
            _lblCurrentDate.LoadContent(content);

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