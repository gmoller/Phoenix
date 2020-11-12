using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class FoodView : CommodityView
    {
        public FoodView(string name, Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2) :
            base(position, positionAlignment, settlementView, imageTextureName1, imageTextureName2, name)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var x = Draw(spriteBatch, Area.X, Area.Y, SettlementView.Settlement.FoodSubsistence);
            x += 20;
            Draw(spriteBatch, x, Area.Y, SettlementView.Settlement.FoodSurplus);
        }
    }
}