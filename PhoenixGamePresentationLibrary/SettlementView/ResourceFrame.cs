using Microsoft.Xna.Framework;
using GuiControls;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class ResourceFrame
    {
        private readonly SettlementView _parent;

        private readonly FrameDynamicSizing _smallFrame;
        private readonly Label _lblResources;
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

        internal ResourceFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;

            _smallFrame = new FrameDynamicSizing(topLeftPosition + new Vector2(0.0f, 10.0f), new Vector2(515, 160), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);

            _lblResources = new Label("lblResources", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);
            _lblFood1 = new Label("lblFood1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblFood2 = new Label("lblFood2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblProduction1 = new Label("lblProduction1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblProduction2 = new Label("lblProduction2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblGold1 = new Label("lblGold1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 100.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblGold2 = new Label("lblGold2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 100.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblPower1 = new Label("lblPower1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 130.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblPower2 = new Label("lblPower2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 130.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblResearch1 = new Label("lblResearch1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 160.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblResearch2 = new Label("lblResearch2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 160.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            _lblResources.Text = "Resources";
            _lblFood1.Text = "Food";
            _lblFood2.Text = $"{_parent.Settlement.SettlementFoodProduction}";
            _lblProduction1.Text = "Production";
            _lblProduction2.Text = $"{_parent.Settlement.SettlementProduction}";
            _lblGold1.Text = "Gold";
            _lblGold2.Text = $"{0}";
            _lblPower1.Text = "Power";
            _lblPower2.Text = $"{0}";
            _lblResearch1.Text = "Research";
            _lblResearch2.Text = $"{0}";
        }

        internal void Draw()
        {
            _smallFrame.Draw();
            _lblResources.Draw();
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
}