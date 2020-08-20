using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class ResourceFrame
    {
        private readonly SettlementView _parent;

        private readonly Vector2 _topLeftPosition;
        private FoodView _foodView;
        private ProductionView _productionView;

        internal ResourceFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
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
            _foodView.Draw(spriteBatch);
            _productionView.Draw(spriteBatch);
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
            _image1 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(30.0f, 30.0f), "Icons_1", "Bread", "image1");
            _image1.LoadContent(content);
            _image2 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 20.0f), "Icons_1", "Corn", "image2");
            _image2.LoadContent(content);
        }

        internal void Update(float deltaTime, InputHandler input)
        {
            if (_area.Contains(input.MousePosition))
            {
                //_toolTip = new ToolTip(input.MousePosition.ToVector2() + new Vector2(0.0f, 30.0f));
                //_toolTip.LoadContent(_content);
                //_toolTip.AddControl(new Image(new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(25.0f, 25.0f), "Icons_1", "Bread"));
                //_toolTip.AddControl(new Image(new Vector2(0.0f, 25.0f), Alignment.TopLeft, new Vector2(25.0f, 25.0f), "Icons_1", "Pickaxe"));
                //_toolTip.AddControl(new LabelAutoSized(new Vector2(0.0f, 50.0f), Alignment.TopLeft, "Here is some text!", "CrimsonText-Regular-12", Color.Blue, Color.Red));
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

            for (var i = 0; i < numberOfBread; ++i)
            {
                _image1.SetTopLeftPosition(x, y);
                _image1.Draw(spriteBatch);
                x += 30;
            }

            for (var i = 0; i < numberOfCorn; ++i)
            {
                _image2.SetTopLeftPosition(x, y);
                _image2.Draw(spriteBatch);
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

        internal ProductionView(Vector2 topLeftPosition, SettlementView parent)
        {
            _topLeftPosition = topLeftPosition;
            _parent = parent;
        }

        internal void LoadContent(ContentManager content)
        {
            _image1 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(30.0f, 30.0f), "Icons_1", "Anvil", "image1");
            _image1.LoadContent(content);
            _image2 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 20.0f), "Icons_1", "Pickaxe", "image2");
            _image2.LoadContent(content);
        }

        internal void Update(float deltaTime, InputHandler input)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var x = (int)_topLeftPosition.X;
            var y = (int)_topLeftPosition.Y;

            Draw(spriteBatch, x, y, _parent.Settlement.SettlementProduction);
        }

        private void Draw(SpriteBatch spriteBatch, int x, int y, int production)
        {
            var numberOfAnvils = production / 10;
            var numberOfPickaxes = production % 10;

            for (var i = 0; i < numberOfAnvils; ++i)
            {
                _image1.SetTopLeftPosition(x, y);
                _image1.Draw(spriteBatch);
                x += 30;
            }

            for (var i = 0; i < numberOfPickaxes; ++i)
            {
                _image2.SetTopLeftPosition(x, y);
                _image2.Draw(spriteBatch);
                x += 20;
            }
        }
    }
}