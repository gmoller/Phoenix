using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AssetsLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class UnitsStackViews : IEnumerable<UnitsStackView>
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

        public UnitsStackView Selected
        {
            get => GetSelected();
            set => _selectedUnitStack = value.Id;
        }

    public int Count => _unitsStackViews.Count;

        public UnitsStackView this[int index] => _unitsStackViews[index];

        internal UnitsStackViews(WorldView worldView, UnitsStacks unitsStacks)
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
            //var locationsAlreadyDrawnTo = new Dictionary<Point, bool>();
            foreach (var unitsStackView in _unitsStackViews)
            {
                //if (!locationsAlreadyDrawnTo.ContainsKey(unitsStackView.Location))
                {
                    unitsStackView.Draw(spriteBatch);
                    //locationsAlreadyDrawnTo.Add(unitsStackView.Location, true);
                }
            }
        }

        internal void SelectNext()
        {
            int counter = 0;
            bool leaveLoop = false;
            bool stackFound =  false;
            do
            {
                _selectedUnitStack++;
                if (_selectedUnitStack < _unitsStackViews.Count)
                {
                    var selectedUnitStack = _unitsStackViews[_selectedUnitStack];
                    if (selectedUnitStack.MovementPoints > 0.0f && !selectedUnitStack.IsBusy)
                    {
                        // stack found
                        stackFound = true;
                        leaveLoop = true;
                    }
                }
                else
                {
                    _selectedUnitStack = -1;
                }

                counter++;
                if (counter > _unitsStackViews.Count)
                {
                    // no stack found - all busy
                    stackFound = false;
                    leaveLoop = true;
                }
            } while (!leaveLoop);


            if (stackFound)
            {
                var unitsStackView = _unitsStackViews[_selectedUnitStack];
                unitsStackView.SetButtons();
                _worldView.Camera.LookAtCell(unitsStackView.Location);
            }
            else
            {
                _selectedUnitStack = -1;
            }
        }

        private UnitsStackView GetSelected()
        {
            if (_selectedUnitStack < 0 || _selectedUnitStack > _unitsStackViews.Count - 1) return null;

            var unitsStackView = _unitsStackViews[_selectedUnitStack];

            return unitsStackView;
        }

        private void CreateNewUnitsStackView(WorldView worldView, UnitsStack unitsStack)
        {
            var unitsStackView = new UnitsStackView(worldView, this, unitsStack, _unitsStackViews.Count);
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