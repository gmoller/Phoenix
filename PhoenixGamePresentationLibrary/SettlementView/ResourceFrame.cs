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
        private Frame _smallFrame;
        private LabelAutoSized _lblResources;
        private LabelAutoSized _lblFood;
        private LabelAutoSized _lblProduction;
        private LabelAutoSized _lblGold;
        private LabelAutoSized _lblPower;
        private LabelAutoSized _lblResearch;
        private FoodView _foodView;
        private ProductionView _productionView;

        internal ResourceFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _smallFrame = new Frame("SmallFrame", _topLeftPosition + new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(515, 175), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);
            _smallFrame.LoadContent(content);

            _lblResources = new LabelAutoSized("_lblResources", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y), Alignment.TopLeft, "Resources", "CrimsonText-Regular-12", Color.Orange, Color.DarkBlue);
            _lblResources.LoadContent(content);

            _lblFood = new LabelAutoSized("_lblFood", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, "Food", "CrimsonText-Regular-12", Color.Orange);
            _lblFood.LoadContent(content);
            _lblProduction = new LabelAutoSized("_lblProduction", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, "Production", "CrimsonText-Regular-12", Color.Orange);
            _lblProduction.LoadContent(content);
            _lblGold = new LabelAutoSized("_lblGold", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 85.0f), Alignment.TopLeft, "Gold", "CrimsonText-Regular-12", Color.Orange);
            _lblGold.LoadContent(content);
            _lblPower = new LabelAutoSized("_lblPower", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 115.0f), Alignment.TopLeft, "Power", "CrimsonText-Regular-12", Color.Orange);
            _lblPower.LoadContent(content);
            _lblResearch = new LabelAutoSized("_lblResearch", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 145.0f), Alignment.TopLeft, "Research", "CrimsonText-Regular-12", Color.Orange);
            _lblResearch.LoadContent(content);

            _foodView = new FoodView(new Vector2(_topLeftPosition.X + 150.0f, _topLeftPosition.Y + 20.0f), _parent);
            _foodView.LoadContent(content);
            _productionView = new ProductionView(new Vector2(_topLeftPosition.X + 150.0f, _topLeftPosition.Y + 50.0f), _parent);
            _productionView.LoadContent(content);
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
            _lblResources.Draw();
            _lblFood.Draw();
            _lblProduction.Draw();
            _lblGold.Draw();
            _lblPower.Draw();
            _lblResearch.Draw();

            _foodView.Draw(spriteBatch);
            _productionView.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

    internal class FoodView
    {
        private readonly Vector2 _topLeftPosition;
        private readonly SettlementView _parent;
        private Image _image1;
        private Image _image2;

        private ToolTip _toolTip;

        private Rectangle _area;

        internal FoodView(Vector2 topLeftPosition, SettlementView parent)
        {
            _topLeftPosition = topLeftPosition;
            _parent = parent;

            _area = new Rectangle();
        }

        internal void LoadContent(ContentManager content)
        {
            _image1 = new Image("Image1", Vector2.Zero, Alignment.TopLeft, new Vector2(30.0f, 30.0f), "Icons_1", "Bread");
            _image1.LoadContent(content);
            _image2 = new Image("Image2", Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 20.0f), "Icons_1", "Corn");
            _image2.LoadContent(content);
        }

        internal void Update(float deltaTime, InputHandler input)
        {
            if (_area.Contains(input.MousePosition))
            {
                _toolTip = new ToolTip(input.MousePosition.ToVector2() + new Vector2(0.0f, 30.0f));
                _toolTip.AddControl(new Image("Test1", new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(25.0f, 25.0f), "Icons_1", "Bread"));
                _toolTip.AddControl(new Image("Test2", new Vector2(0.0f, 25.0f), Alignment.TopLeft, new Vector2(25.0f, 25.0f), "Icons_1", "Pickaxe"));
                _toolTip.AddControl(new LabelAutoSized("Test3", new Vector2(0.0f, 50.0f), Alignment.TopLeft, "Here is some text!", "CrimsonText-Regular-12", Color.Blue, Color.Red));
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
            var numberOfCorn = food % 10;

            for (int i = 0; i < numberOfBread; ++i)
            {
                _image1.SetTopLeftPosition(x, y);
                _image1.Draw();
                x += 30;
            }

            for (int i = 0; i < numberOfCorn; ++i)
            {
                _image2.SetTopLeftPosition(x, y);
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
        private  Image _image1;
        private  Image _image2;

        private Rectangle _area;

        internal ProductionView(Vector2 topLeftPosition, SettlementView parent)
        {
            _topLeftPosition = topLeftPosition;
            _parent = parent;

            _area = new Rectangle();
        }

        internal void LoadContent(ContentManager content)
        {
            _image1 = new Image("Image1", Vector2.Zero, Alignment.TopLeft, new Vector2(30.0f, 30.0f), "Icons_1", "Anvil");
            _image1.LoadContent(content);
            _image2 = new Image("Image2", Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 20.0f), "Icons_1", "Pickaxe");
            _image2.LoadContent(content);
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
            var numberOfPickaxes = production % 10;

            for (int i = 0; i < numberOfAnvils; ++i)
            {
                _image1.SetTopLeftPosition(x, y);
                _image1.Draw();
                x += 30;
            }

            for (int i = 0; i < numberOfPickaxes; ++i)
            {
                _image2.SetTopLeftPosition(x, y);
                _image2.Draw();
                x += 20;
            }

            return x;
        }
    }
}