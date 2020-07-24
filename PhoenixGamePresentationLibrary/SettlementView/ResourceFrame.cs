using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class ResourceFrame
    {
        private readonly SettlementView _parent;

        private readonly Vector2 _topLeftPosition;
        private FrameDynamicSizing _smallFrame;
        private Label _lblResources;
        private Label _lblFood;
        private Label _lblProduction;
        private Label _lblGold;
        private Label _lblPower;
        private Label _lblResearch;
        private FoodView _foodView;
        private ProductionView _productionView;

        internal ResourceFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _smallFrame = new FrameDynamicSizing(_topLeftPosition + new Vector2(0.0f, 0.0f), new Vector2(515, 175), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);
            _smallFrame.LoadContent(content);

            _lblResources = new Label("lblResources", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Resources", HorizontalAlignment.Left, Color.Orange, Color.Red);
            _lblFood = new Label("lblFood", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 30.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Food", HorizontalAlignment.Left, Color.Orange);
            _lblProduction = new Label("lblProduction", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 60.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Production", HorizontalAlignment.Left, Color.Orange);
            _lblGold = new Label("lblGold", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 90.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Gold", HorizontalAlignment.Left, Color.Orange);
            //_lblGold2 = new Label("lblGold2", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 200.0f, _topLeftPosition.Y + 100.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblPower = new Label("lblPower", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 120.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Power", HorizontalAlignment.Left, Color.Orange);
            //_lblPower2 = new Label("lblPower2", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 200.0f, _topLeftPosition.Y + 130.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblResearch = new Label("lblResearch", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 150.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Research", HorizontalAlignment.Left, Color.Orange);
            //_lblResearch2 = new Label("lblResearch2", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 200.0f, _topLeftPosition.Y + 160.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);

            _foodView = new FoodView(new Vector2(_topLeftPosition.X + 150.0f, _topLeftPosition.Y + 20.0f), _parent);
            _productionView = new ProductionView(new Vector2(_topLeftPosition.X + 150.0f, _topLeftPosition.Y + 50.0f), _parent);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _foodView.Update(deltaTime, input);
            _productionView.Update(deltaTime, input);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _smallFrame.Draw();
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
        private readonly Vector2 _topLeftPosition;
        private readonly SettlementView _parent;
        private readonly Image _image1;
        private readonly Image _image2;

        private ToolTip _toolTip;

        private Rectangle _area;

        internal FoodView(Vector2 topLeftPosition, SettlementView parent)
        {
            _topLeftPosition = topLeftPosition;
            _parent = parent;

            _area = new Rectangle();

            _image1 = new Image("Image1", Vector2.Zero, new Vector2(30.0f, 30.0f), "Icons_1", "Bread");
            _image2 = new Image("Image2", Vector2.Zero, new Vector2(20.0f, 20.0f), "Icons_1", "Corn");
        }

        internal void Update(float deltaTime, InputHandler input)
        {
            if (_area.Contains(input.MousePosition))
            {
                _toolTip = new ToolTip(input.MousePosition.ToVector2() + new Vector2(0.0f, 30.0f));
                _toolTip.AddControl(new Image("Test1", new Vector2(0.0f, 0.0f), new Vector2(25.0f, 25.0f), "Icons_1", "Bread"));
                _toolTip.AddControl(new Image("Test2", new Vector2(0.0f, 25.0f), new Vector2(25.0f, 25.0f), "Icons_1", "Pickaxe"));
                _toolTip.AddControl(new Label2("Test3", new Vector2(0.0f, 50.0f), ContentAlignment.TopLeft, new Vector2(89.0f, 12.0f), "Here is some text!", "CrimsonText-Regular-12", Color.Blue, Color.Red));
            }
            else
            {
                _toolTip = null;
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

            _toolTip?.Draw();

            _area = new Rectangle(originalX, originalY, x, 30);
        }

        private int Draw(SpriteBatch spriteBatch, int x, int y, int food)
        {
            var numberOfBread = food / 10;
            var numberofCorn = food % 10;

            for (int i = 0; i < numberOfBread; ++i)
            {
                _image1.Position = new Vector2(x, y);
                _image1.Draw();
                x += 30;
            }

            for (int i = 0; i < numberofCorn; ++i)
            {
                _image2.Position = new Vector2(x, y);
                _image2.Draw();
                x += 20;
            }

            return x;
        }
    }

    internal class ProductionView
    {
        private readonly Vector2 _topLeftPosition;
        private readonly SettlementView _parent;
        private readonly Image _image1;
        private readonly Image _image2;

        private Rectangle _area;

        internal ProductionView(Vector2 topLeftPosition, SettlementView parent)
        {
            _topLeftPosition = topLeftPosition;
            _parent = parent;

            _area = new Rectangle();

            _image1 = new Image("Image1", Vector2.Zero, new Vector2(30.0f, 30.0f), "Icons_1", "Anvil");
            _image2 = new Image("Image2", Vector2.Zero, new Vector2(20.0f, 20.0f), "Icons_1", "Pickaxe");
        }

        internal void Update(float deltaTime, InputHandler input)
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
                _image1.Position = new Vector2(x, y);
                _image1.Draw();
                x += 30;
            }

            for (int i = 0; i < numberofPickaxes; ++i)
            {
                _image2.Position = new Vector2(x, y);
                _image2.Draw();
                x += 20;
            }

            return x;
        }
    }
}