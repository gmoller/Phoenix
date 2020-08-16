using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    internal class SettlementViews
    {
        #region State
        private readonly WorldView _worldView;

        private readonly Settlements _settlements;
        private readonly List<SettlementView.SettlementView> _settlementViews;

        private ContentManager _content;
        #endregion

        internal SettlementViews(WorldView worldView, Settlements settlements)
        {
            _worldView = worldView;
            _settlements = settlements;
            _settlementViews = new List<SettlementView.SettlementView>();
        }

        internal void LoadContent(ContentManager content)
        {
            foreach (var settlement in _settlements)
            {
                var settlementView = new SettlementView.SettlementView(settlement);
                settlementView.LoadContent(content);
                _settlementViews.Add(settlementView);
            }

            _content = content;
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            while (_settlementViews.Count < _settlements.Count)
            {
                CreateNewSettlementView(_worldView, _settlements[_settlementViews.Count]);
            }

            foreach (var settlementView in _settlementViews)
            {
                settlementView.Update(input, deltaTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (var settlementView in _settlementViews)
            {
                if (settlementView.Settlement.IsSelected)
                {
                    settlementView.Draw(spriteBatch);
                }
            }
        }

        private void CreateNewSettlementView(WorldView worldView, Settlement settlement)
        {
            var settlementView = new SettlementView.SettlementView(settlement);
            settlementView.LoadContent(_content);
            _settlementViews.Add(settlementView);
        }
    }
}