﻿using GuiControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class ProductionView : CommodityView
    {
        internal ProductionView(string name, Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2) :
            base(position, positionAlignment, settlementView, imageTextureName1, imageTextureName2, name)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Area.X, Area.Y, SettlementView.Settlement.SettlementProduction);
        }
    }
}