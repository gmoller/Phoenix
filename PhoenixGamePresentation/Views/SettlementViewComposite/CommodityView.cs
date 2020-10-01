﻿using GuiControls;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PointI = Utilities.PointI;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal abstract class CommodityView : Control
    {
        #region State
        protected readonly SettlementView SettlementView;

        private Image _image1;
        private Image _image2;
        //protected ToolTip ToolTip;
        //protected Rectangle Area2;
        #endregion State

        internal CommodityView(Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2, string name) :
            base(position, positionAlignment, new Vector2(100.0f, 30.0f), name)
        {
            SettlementView = settlementView;

            _image1 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(30.0f, 30.0f), $"Icons_1.{imageTextureName1}", "image1");
            _image2 = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 20.0f), $"Icons_1.{imageTextureName2}", "image2");
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            _image1.LoadContent(content);
            _image2.LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
        }

        public abstract override void Draw(SpriteBatch spriteBatch);

        protected int Draw(SpriteBatch spriteBatch, int x, int y, int commodity)
        {
            var numberOfItem1 = commodity / 10;
            var numberOfItem2 = commodity % 10;

            for (var i = 0; i < numberOfItem1; ++i)
            {
                _image1.SetTopLeftPosition(new PointI(x, y));
                _image1.Draw(spriteBatch);
                x += 30;
            }

            for (var i = 0; i < numberOfItem2; ++i)
            {
                _image2.SetTopLeftPosition(new PointI(x, y));
                _image2.Draw(spriteBatch);
                x += 20;
            }

            return x;
        }
    }
}