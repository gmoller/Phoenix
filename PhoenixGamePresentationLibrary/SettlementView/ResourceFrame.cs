using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class ResourceFrame
    {
        private readonly SettlementView _parent;

        private readonly Vector2 _topLeftPosition;
        private readonly FrameDynamicSizing _smallFrame;
        private readonly Label _lblResources;
        private readonly Label _lblFood;
        private readonly Label _lblProduction;
        private readonly Label _lblGold;
        private readonly Label _lblPower;
        private readonly Label _lblResearch;
        private readonly FoodView _foodView;
        private readonly ProductionView _productionView;

        internal ResourceFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;

            _smallFrame = new FrameDynamicSizing(topLeftPosition + new Vector2(0.0f, 0.0f), new Vector2(515, 175), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);

            _lblResources = new Label("lblResources", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Resources", HorizontalAlignment.Left, Color.Orange, Color.Red);
            _lblFood = new Label("lblFood", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 30.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Food", HorizontalAlignment.Left, Color.Orange);
            _lblProduction = new Label("lblProduction", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 60.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Production", HorizontalAlignment.Left, Color.Orange);
            _lblGold = new Label("lblGold", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 90.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Gold", HorizontalAlignment.Left, Color.Orange);
            //_lblGold2 = new Label("lblGold2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 100.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblPower = new Label("lblPower", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 120.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Power", HorizontalAlignment.Left, Color.Orange);
            //_lblPower2 = new Label("lblPower2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 130.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblResearch = new Label("lblResearch", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 150.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Research", HorizontalAlignment.Left, Color.Orange);
            //_lblResearch2 = new Label("lblResearch2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 160.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);

            _foodView = new FoodView(new Vector2(topLeftPosition.X + 150.0f, topLeftPosition.Y + 20.0f), parent);
            _productionView = new ProductionView(new Vector2(topLeftPosition.X + 150.0f, topLeftPosition.Y + 50.0f), parent);
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            _foodView.Update(gameTime, input);
            _productionView.Update(gameTime, input);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _smallFrame.Draw(spriteBatch);
            _lblResources.Draw(spriteBatch);
            _lblFood.Draw(spriteBatch);
            _lblProduction.Draw(spriteBatch);
            _lblGold.Draw(spriteBatch);
            _lblPower.Draw(spriteBatch);
            _lblResearch.Draw(spriteBatch);

            _foodView.Draw(spriteBatch);
            _productionView.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

    internal class FoodView
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _topLeftPosition;
        private readonly SettlementView _parent;

        private FrameDynamicSizing _toolTip;

        private Rectangle _area;

        internal FoodView(Vector2 topLeftPosition, SettlementView parent)
        {
            _texture = AssetsManager.Instance.GetTexture("Icons_1");
            _topLeftPosition = topLeftPosition;
            _parent = parent;

            _area = new Rectangle();
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            if (_area.Contains(input.MousePostion))
            {
                _toolTip = new FrameDynamicSizing(input.MousePostion.ToVector2(), new Vector2(100.0f, 100.0f), "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);
                //_toolTip.AddControl(new Label("lblFood"));
                //_toolTip.AddControl(new Image());
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var x = (int)_topLeftPosition.X;
            var y = (int)_topLeftPosition.Y;
            var originalX = x;
            var originalY = y;

            x = Draw(spriteBatch, x, y, _parent.Settlement.FoodSubsistence);
            x += 20;
            x = Draw(spriteBatch, x, y, _parent.Settlement.FoodSurplus);

            _area = new Rectangle(originalX, originalY, x, 30);
        }

        private int Draw(SpriteBatch spriteBatch, int x, int y, int food)
        {
            var numberOfBread = food / 10;
            var numberofCorn = food % 10;

            for (int i = 0; i < numberOfBread; ++i)
            {
                var image = new Image("1", new Vector2(x, y), new Vector2(30, 30), "Icons_1", "Bread");
                image.Draw(spriteBatch);
                x += 30;
            }

            for (int i = 0; i < numberofCorn; ++i)
            {
                var image = new Image("1", new Vector2(x, y + 2), new Vector2(20, 20), "Icons_1", "Corn");
                image.Draw(spriteBatch);
                x += 20;
            }

            return x;
        }
    }

    internal class ProductionView
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _topLeftPosition;
        private readonly SettlementView _parent;

        private Rectangle _area;

        internal ProductionView(Vector2 topLeftPosition, SettlementView parent)
        {
            _texture = AssetsManager.Instance.GetTexture("Icons_1");
            _topLeftPosition = topLeftPosition;
            _parent = parent;

            _area = new Rectangle();
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var x = (int)_topLeftPosition.X;
            var y = (int)_topLeftPosition.Y;
            var originalX = x;
            var originalY = y;

            x = Draw(spriteBatch, x, y, _parent.Settlement.SettlementProduction);

            _area = new Rectangle(originalX, originalY, x, 30);
        }

        private int Draw(SpriteBatch spriteBatch, int x, int y, int production)
        {
            var numberOfAnvils = production / 10;
            var numberofPickaxes = production % 10;

            for (int i = 0; i < numberOfAnvils; ++i)
            {
                var image = new Image("1", new Vector2(x, y), new Vector2(30, 30), "Icons_1", "Anvil");
                image.Draw(spriteBatch);
                x += 30;
            }

            for (int i = 0; i < numberofPickaxes; ++i)
            {
                var image = new Image("1", new Vector2(x, y + 2), new Vector2(20, 20), "Icons_1", "Pickaxe");
                image.Draw(spriteBatch);
                x += 20;
            }

            return x;
        }
    }
}