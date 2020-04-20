using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    internal class SettlementsView
    {
        private readonly Settlements _settlements;

        private readonly List<SettlementView.SettlementView> _settlementViews;

        internal SettlementsView(Settlements settlements)
        {
            _settlements = settlements;
            _settlementViews = new List<SettlementView.SettlementView>();
        }

        internal void Update(float deltaTime)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _settlementViews)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}