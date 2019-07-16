using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLogic;
using GuiControls;
using Utilities;

namespace PhoenixGameLibrary.Views
{
    public class HudView
    {
        private FrameDynamicSizing _frame;

        private Label _lblCurrentDate;
        private Button _btnGold;
        private Label _lblGold;
        private Button _btnMana;
        private Label _lblMana;
        private Button _btnFood;
        private Label _lblFood;

        public void LoadContent(ContentManager content)
        {
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
        }

        public void Draw()
        {
            _frame.Draw();

            _lblCurrentDate.Draw();

            _btnGold.Draw();
            _lblGold.Draw();

            _btnMana.Draw();
            _lblMana.Draw();

            _btnFood.Draw();
            _lblFood.Draw();
        }
    }
}