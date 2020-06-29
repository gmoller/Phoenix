using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    internal class SettlementsView
    {
        private readonly WorldView _worldView;
        private readonly Settlements _settlements;
        private ContentManager _content;

        private List<SettlementView.SettlementView> _settlementViews;

        internal SettlementsView(WorldView worldView, Settlements settlements)
        {
            _worldView = worldView;
            _settlements = settlements;
        }

        public void LoadContent(ContentManager content)
        {
            _content = content;
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _settlementViews = new List<SettlementView.SettlementView>();
            foreach (var settlement in _settlements)
            {
                var settlementView = new SettlementView.SettlementView(_worldView, settlement);
                settlementView.LoadContent(_content);
                settlementView.Update(input, deltaTime);
                _settlementViews.Add(settlementView);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (var settlement in _settlementViews)
            {
                if (settlement.Settlement.IsSelected)
                {
                    settlement.Draw(spriteBatch);
                }
            }
        }
    }
}