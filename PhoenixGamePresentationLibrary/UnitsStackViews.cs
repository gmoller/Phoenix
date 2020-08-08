using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
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

        private Queue<UnitsStackView> _ordersQueue;
        private readonly List<Guid> _selectedThisTurn;

        internal Texture2D GuiTextures { get; private set; }
        internal AtlasFrame SquareGreenFrame { get; private set; }
        internal AtlasFrame SquareGrayFrame { get; private set; }
        internal Texture2D UnitTextures { get; private set; }
        internal AtlasSpec2 UnitAtlas { get; private set; }

        internal UnitsStackView Current { get; private set; }

        public int Count => _unitsStackViews.Count;

        public UnitsStackView this[int index] => _unitsStackViews[index];

        internal UnitsStackViews(WorldView worldView, UnitsStacks unitsStacks)
        {
            _worldView = worldView;
            _unitsStacks = unitsStacks;
            _unitsStackViews = new List<UnitsStackView>();
            Current = null;
            _ordersQueue = new Queue<UnitsStackView>();
            _selectedThisTurn = new List<Guid>();
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

        internal void BeginTurn()
        {
            // create a queue of stacks that need orders
            var queue = new Queue<UnitsStackView>();
            foreach (var unitStackView in _unitsStackViews)
            {
                if (!unitStackView.IsBusy) // not patrol, or fortify
                {
                    queue.Enqueue(unitStackView);
                }
            }

            _ordersQueue = queue;
            _selectedThisTurn.Clear();

            SelectNext();
        }

        internal void DoWaitAction()
        {
            _ordersQueue.Enqueue(Current);
            SelectNext();
        }

        internal void DoDoneAction()
        {
            SelectNext();
        }

        internal void DoPatrolAction()
        {
            Current.DoPatrolAction();
            SelectNext();
        }

        internal void SetCurrent(UnitsStackView unitsStackView)
        {
            _selectedThisTurn.Add(unitsStackView.Id);
            if (Current != null)
            {
                _ordersQueue.Enqueue(Current);
            }

            Current = unitsStackView;
            Current.SetStatusToNone();
        }

        internal void SelectNext()
        {
            if (_ordersQueue.Count > 0)
            {
                Current = _ordersQueue.Dequeue();
                if (_selectedThisTurn.Contains(Current.Id))
                {
                    SelectNext();
                }
                else
                {
                    Current.SetButtons();
                    _worldView.Camera.LookAtCell(Current.Location);
                }
            }
            else
            {
                Current = null;
            }
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