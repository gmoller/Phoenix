using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AssetsLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;
using System;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class UnitsStacksView : IEnumerable<UnitsStackView>
    {
        private readonly WorldView _worldView;

        private readonly UnitsStacks _unitsStacks;
        private readonly List<UnitsStackView> _unitsStackViews;
        private int _selectedUnitStack;

        internal Texture2D GuiTextures { get; private set; }
        internal AtlasFrame SquareGreenFrame { get; private set; }
        internal AtlasFrame SquareGrayFrame { get; private set; }
        internal Texture2D UnitTextures { get; private set; }
        internal AtlasSpec2 UnitAtlas { get; private set; }

        public UnitsStackView Selected => GetSelected();

        public int Count => _unitsStackViews.Count;

        public UnitsStackView this[int index] => _unitsStackViews[index];

        internal UnitsStacksView(WorldView worldView, UnitsStacks unitsStacks)
        {
            _worldView = worldView;
            _unitsStacks = unitsStacks;
            _unitsStackViews = new List<UnitsStackView>();
            _selectedUnitStack = -1;
        }

        internal void LoadContent(ContentManager content)
        {
            var atlas = AssetsManager.Instance.GetAtlas("Squares");
            GuiTextures = AssetsManager.Instance.GetTexture("Squares");
            SquareGreenFrame = atlas.Frames["SquareGreen"];
            SquareGrayFrame = atlas.Frames["SquareGray"];

            UnitAtlas = AssetsManager.Instance.GetAtlas("Units");
            UnitTextures = AssetsManager.Instance.GetTexture("Units");

            foreach (var unitsStack in _unitsStacks)
            {
                CreateNewUnitsStackView(_worldView, unitsStack);
            }
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            while (_unitsStackViews.Count < _unitsStacks.Count)
            {
                CreateNewUnitsStackView(_worldView, _unitsStacks[_unitsStackViews.Count]);
            }

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

        internal void SelectNext()
        {
            _selectedUnitStack++;
            if (_unitsStackViews.Count <= _selectedUnitStack)
            {
                _selectedUnitStack = -1;
            }
            else
            {
                if (_unitsStackViews[_selectedUnitStack].MovementPoints <= 0.0f)
                {
                    SelectNext();
                }
            }
        }

        private UnitsStackView GetSelected()
        {
            if (_selectedUnitStack < 0 || _selectedUnitStack > _unitsStackViews.Count - 1) return null;

            var unitsStackView = _unitsStackViews[_selectedUnitStack];
            _worldView.Camera.LookAtCell(unitsStackView.Location);

            return unitsStackView;
        }

        private void CreateNewUnitsStackView(WorldView worldView, UnitsStack unitsStack)
        {
            var unitsStackView = new UnitsStackView(worldView, this, unitsStack);
            _unitsStackViews.Add(unitsStackView);
        }

        public IEnumerator<UnitsStackView> GetEnumerator()
        {
            foreach (var item in _unitsStackViews)
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

        private string DebuggerDisplay => $"{{Count={_unitsStackViews.Count}}}";
    }
}