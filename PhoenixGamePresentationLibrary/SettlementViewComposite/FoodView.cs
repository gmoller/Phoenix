using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class FoodView : CommodityView
    {
        public FoodView(Vector2 position, Alignment positionAlignment, SettlementView settlementView, string imageTextureName1, string imageTextureName2, string name, IControl parent = null) :
            base(position, positionAlignment, settlementView, imageTextureName1, imageTextureName2, name, parent)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var x = Area.X;
            var y = Area.Y;
            var originalX = x;
            var originalY = y;

            x = Draw(spriteBatch, x, y, SettlementView.Settlement.FoodSubsistence);
            x += 20;
            x = Draw(spriteBatch, x, y, SettlementView.Settlement.FoodSurplus);

            //ToolTip?.Draw();

            //Area2 = new Rectangle(originalX, originalY, x, 30);
        }
    }
}