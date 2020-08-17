using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class SettlementViews : IEnumerable<SettlementView>
    {
        #region State
        private readonly WorldView _worldView;

        private readonly Settlements _settlements;
        private readonly List<SettlementView> _settlementViews;

        private ContentManager _content;
        #endregion

        public int Count => _settlementViews.Count;

        public SettlementView this[int index] => _settlementViews[index];

        internal SettlementViews(WorldView worldView, Settlements settlements)
        {
            _worldView = worldView;
            _settlements = settlements;
            _settlementViews = new List<SettlementView>();
        }

        internal void LoadContent(ContentManager content)
        {
            foreach (var settlement in _settlements)
            {
                CreateNewSettlementView(_worldView, settlement);
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
            var settlementView = new SettlementView(settlement);
            settlementView.LoadContent(_content);
            _settlementViews.Add(settlementView);
        }

        public IEnumerator<SettlementView> GetEnumerator()
        {
            foreach (var item in _settlementViews)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_settlementViews.Count}}}";
    }
}