using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class ProductionView : CommodityView
    {
        internal ProductionView(string name, Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2) :
            base(position, positionAlignment, settlementView, imageTextureName1, imageTextureName2, name)
        {
        }

        public override IControl Clone()
        {
            throw null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Bounds.X, Bounds.Y, SettlementView.Settlement.SettlementProduction);
        }
    }
}