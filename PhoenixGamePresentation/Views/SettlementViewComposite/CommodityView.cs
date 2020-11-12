using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;
using Zen.Input;
using Zen.MonoGameUtilities.ExtensionMethods;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal abstract class CommodityView : Control
    {
        #region State
        protected readonly SettlementView SettlementView;
        private Controls Controls { get; }
        #endregion State

        internal CommodityView(Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2, string name) :
            base(name)
        {
            Size = new PointI(100, 30);
            PositionAlignment = positionAlignment;
            SetPosition(position.ToPointI());

            SettlementView = settlementView;

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textureName1", $"Icons_1.{imageTextureName1}"),
                new KeyValuePair<string, string>("textureName2", $"Icons_1.{imageTextureName2}")
            };

            Controls = ControlCreator.CreateFromSpecification(
                @"
image1 : Image
{
  TextureName: %textureName1%
  Size: 30;30
}

image2 : Image
{
  TextureName: %textureName2%
  Size: 20;20
}", pairs);
            Controls.SetOwner(this);
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            foreach (var control in Controls)
            {
                control.LoadContent(content, true);
            }
        }

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
        }

        public abstract override void Draw(SpriteBatch spriteBatch);

        protected int Draw(SpriteBatch spriteBatch, int x, int y, int commodity)
        {
            var numberOfItem1 = commodity / 10;
            var numberOfItem2 = commodity % 10;

            for (var i = 0; i < numberOfItem1; i++)
            {
                var image = (Image)Controls["image1"];
                image.SetPosition(new PointI(x, y));
                image.Draw(spriteBatch);
                x += image.Width;
            }

            for (var i = 0; i < numberOfItem2; i++)
            {
                var image = (Image)Controls["image2"];
                image.SetPosition(new PointI(x, y));
                image.Draw(spriteBatch);
                x += image.Width;
            }

            return x;
        }
    }
}