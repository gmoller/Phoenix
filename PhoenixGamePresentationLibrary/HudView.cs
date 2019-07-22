using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using PhoenixGameLibrary.Commands;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class HudView
    {
        private SpriteFont _font;
        private FrameDynamicSizing _frame;

        private Label _lblCurrentDate;
        private Button _btnGold;
        private Label _lblGold;
        private Button _btnMana;
        private Label _lblMana;
        private Button _btnFood;
        private Label _lblFood;

        private Button _btnEndTurn;

        internal void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            var topLeftPosition = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width - 250.0f, 0.0f);
            var size = new Vector2(250.0f, DeviceManager.Instance.GraphicsDevice.Viewport.Height - 60);
            _frame = new FrameDynamicSizing(topLeftPosition, size, "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);

            var topCenterPosition = new Vector2(topLeftPosition.X + size.X / 2, topLeftPosition.Y) + new Vector2(0.0f, 10.0f);
            _lblCurrentDate = new Label("lblCurrentDate", "Maleficio-Regular-18", topCenterPosition, HorizontalAlignment.Center, VerticalAlignment.Top, Vector2.Zero, "Date:", HorizontalAlignment.Center, Color.Aquamarine);

            var position = new Vector2(topLeftPosition.X + 20.0f, 200.0f);
            _btnGold = new Button("btnGold", position, HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(50.0f, 50.0f), "Icons_1", "Coin_T", "Coin_R", "Coin_R", "Coin_R");
            _lblGold = new Label("lblGold", "CrimsonText-Regular-12", new Vector2(_btnGold.Right + 20.0f, _btnGold.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack);

            _btnMana = new Button("btnMana", _btnGold.BottomLeft + new Vector2(0.0f, 10.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(50.0f, 50.0f), "Icons_1", "Potion_T", "Potion_R", "Potion_R", "Potion_R");
            _lblMana = new Label("lblMana", "CrimsonText-Regular-12", new Vector2(_btnMana.Right + 20.0f, _btnMana.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack);

            _btnFood = new Button("btnFood", _btnMana.BottomLeft + new Vector2(0.0f, 10.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(50.0f, 50.0f), "Icons_1", "Bread_T", "Bread_R", "Bread_R", "Bread_R");
            _lblFood = new Label("lblFood", "CrimsonText-Regular-12", new Vector2(_btnFood.Right + 20.0f, _btnFood.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack);

            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var label = new Label("lblNextTurn", "CrimsonText-Regular-12", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue);
            _btnEndTurn = new Button("btnEndTurn", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_a", "reg_button_h", label);
            _btnEndTurn.Click += btnEndTurnClick;
        }

        public void Update(GameTime gameTime)
        {
            _lblCurrentDate.Update(gameTime);
            _lblCurrentDate.Text = Globals.Instance.World.CurrentDate;

            _btnGold.Update(gameTime);
            _lblGold.Update(gameTime);
            _lblGold.Text = $"{Globals.Instance.World.PlayerFaction.GoldInTreasury} GP (+{Globals.Instance.World.PlayerFaction.GoldPerTurn})";

            _btnMana.Update(gameTime);
            _lblMana.Update(gameTime);
            _lblMana.Text = "5 MP (+1)";

            _btnFood.Update(gameTime);
            _lblFood.Update(gameTime);
            _lblFood.Text = $"{Globals.Instance.World.PlayerFaction.FoodPerTurn} Food";

            _btnEndTurn.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _frame.Draw(spriteBatch);
            spriteBatch.End();

            _lblCurrentDate.Draw();

            _btnGold.Draw();
            _lblGold.Draw();

            _btnMana.Draw();
            _lblMana.Draw();

            _btnFood.Draw();
            _lblFood.Draw();

            spriteBatch.Begin();
            DrawNotifications(spriteBatch);
            DrawTileInfo(spriteBatch);
            spriteBatch.End();

            _btnEndTurn.Draw();
        }

        private void DrawNotifications(SpriteBatch spriteBatch)
        {
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - 250.0f + 10.0f;
            var y = DeviceManager.Instance.GraphicsDevice.Viewport.Height / 2.0f;
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

        private void DrawTileInfo(SpriteBatch spriteBatch)
        {
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - 250.0f + 10.0f;
            var y = DeviceManager.Instance.GraphicsDevice.Viewport.Height * 0.90f;

            // get tile mouse is over
            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            var hex = DeviceManager.Instance.WorldHex;
            if (hex.X >= 0 && hex.Y >= 0 && hex.X < PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS && hex.Y < PhoenixGameLibrary.Constants.WORLD_MAP_ROWS)
            {
                var cell = cellGrid.GetCell(hex.X, hex.Y);
                var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
                var text1 = $"{terrainType.Name} - {terrainType.FoodOutput} food";
                spriteBatch.DrawString(_font, text1, new Vector2(x, y), Color.White);

                if (terrainType.CanSettleOn)
                {
                    var catchment = cellGrid.GetCatchment(hex.X, hex.Y);
                    var maxPop = PhoenixGameLibrary.Helpers.BaseFoodLevel.DetermineBaseFoodLevel(new Point(hex.X, hex.Y), catchment);
                    var text2 = $"Maximum Pop - {maxPop}";
                    spriteBatch.DrawString(_font, text2, new Vector2(x, y + 15.0f), Color.White);
                }
            }
        }

        private void btnEndTurnClick(object sender, EventArgs e)
        {
            Globals.Instance.MessageQueue.Enqueue(new EndTurnCommand());
        }
    }
}