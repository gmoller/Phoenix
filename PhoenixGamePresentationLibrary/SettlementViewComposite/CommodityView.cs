using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal abstract class CommodityView : Control
    {
        #region State
        private Image _image1;
        private Image _image2;

        protected readonly SettlementView SettlementView;
        //protected ToolTip ToolTip;
        //protected Rectangle Area2;
        #endregion

        internal CommodityView(Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2, string name, IControl parent = null) :
            base(position, positionAlignment, new Vector2(100.0f, 30.0f), "Icons_1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, name, 0.0f, parent)
        {
            SettlementView = settlementView;

            _image1 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(30.0f, 30.0f), "Icons_1", imageTextureName1, "image1");
            _image2 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 20.0f), "Icons_1", imageTextureName2, "image2");

            //Area2 = new Rectangle();
        }

        public override void LoadContent(ContentManager content)
        {
            _image1.LoadContent(content);
            _image2.LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            //if (Area2.Contains(input.MousePosition))
            //{
            //    ToolTip = new ToolTip(input.MousePosition.ToVector2() + new Vector2(0.0f, 30.0f));
            //    ToolTip.LoadContent(_content);
            //    ToolTip.AddControl(new Image(new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(25.0f, 25.0f), "Icons_1", "Bread"));
            //    ToolTip.AddControl(new Image(new Vector2(0.0f, 25.0f), Alignment.TopLeft, new Vector2(25.0f, 25.0f), "Icons_1", "Pickaxe"));
            //    ToolTip.AddControl(new LabelAutoSized(new Vector2(0.0f, 50.0f), Alignment.TopLeft, "Here is some text!", "CrimsonText-Regular-12", Color.Blue, Color.Red));
            //}
            //else
            //{
            //    ToolTip = null;
            //}
        }

        public abstract override void Draw(SpriteBatch spriteBatch);

        protected int Draw(SpriteBatch spriteBatch, int x, int y, int commodity)
        {
            var numberOfItem1 = commodity / 10;
            var numberOfItem2 = commodity % 10;

            for (var i = 0; i < numberOfItem1; ++i)
            {
                _image1.SetTopLeftPosition(x, y);
                _image1.Draw(spriteBatch);
                x += 30;
            }

            for (var i = 0; i < numberOfItem2; ++i)
            {
                _image2.SetTopLeftPosition(x, y);
                _image2.Draw(spriteBatch);
                x += 20;
            }

            return x;
        }
    }
}