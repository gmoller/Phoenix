using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLogic;
using GuiControls;

namespace PhoenixGameLibrary.Views
{
    public class HudView
    {
        private Button _btnGold;
        private Label _lblGold;
        private Button _btnMana;
        private Label _lblMana;
        private Button _btnFood;
        private Label _lblFood;

        public void LoadContent(ContentManager content)
        {
            _btnGold = new Button("btnGold", new Vector2(0.0f, 0.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(50.0f, 50.0f), "Icons_1", "Coin_T", "Coin_R", "Coin_R");
            _lblGold = new Label("lblGold", "CrimsonText-Regular-12", new Vector2(_btnGold.Right, _btnGold.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack);

            _btnMana = new Button("btnMana", new Vector2(_lblGold.Right, _lblGold.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(50.0f, 50.0f), "Icons_1", "Potion_T", "Potion_R", "Potion_R");
            _lblMana = new Label("lblMana", "CrimsonText-Regular-12", new Vector2(_btnMana.Right, _btnMana.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack);

            _btnFood = new Button("btnFood", new Vector2(_lblMana.Right, _lblMana.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(50.0f, 50.0f), "Icons_1", "Bread_T", "Bread_R", "Bread_R");
            _lblFood = new Label("lblFood", "CrimsonText-Regular-12", new Vector2(_btnFood.Right, _btnFood.Center.Y), HorizontalAlignment.Left, VerticalAlignment.Middle, new Vector2(100.0f, 25.0f), string.Empty, HorizontalAlignment.Left, Color.Yellow, Color.Black, Color.TransparentBlack);
        }

        public void Update(GameTime gameTime)
        {
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
            _btnGold.Draw();
            _lblGold.Draw();

            _btnMana.Draw();
            _lblMana.Draw();

            _btnFood.Draw();
            _lblFood.Draw();
        }
    }
}