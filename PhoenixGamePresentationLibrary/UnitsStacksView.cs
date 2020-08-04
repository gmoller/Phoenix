using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class UnitsStacksView
    {
        private readonly WorldView _worldView;

        private readonly UnitsStacks _unitsStacks;
        private readonly List<UnitsStackView> _unitsStackViews;

        public UnitsStackView Selected
        {
            get
            {
                foreach (var unitsStackView in _unitsStackViews)
                {
                    if (unitsStackView.IsSelected) return unitsStackView;
                }

                return null;
            }
        }

        public int Count => _unitsStacks.Count;

        public UnitsStackView this[int index] => _unitsStackViews[index];

        internal UnitsStacksView(WorldView worldView, UnitsStacks unitsStacks)
        {
            _worldView = worldView;
            _unitsStacks = unitsStacks;
            _unitsStackViews = new List<UnitsStackView>();
        }

        internal void LoadContent(ContentManager content)
        {
            foreach (var unitsStack in _unitsStacks)
            {
                var unitsStackView = new UnitsStackView(_worldView, unitsStack);
                unitsStackView.LoadContent(content);
                _unitsStackViews.Add(unitsStackView);
            }
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            foreach (var unitsStackView in _unitsStackViews)
            {
                unitsStackView.Update(input, deltaTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (var unitsStackView in _unitsStackViews)
            {
                unitsStackView.Draw(spriteBatch);
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_unitsStackViews.Count}}}";
    }
}