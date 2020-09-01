using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Input;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViews : IEnumerable<StackView>
    {
        #region State
        private readonly WorldView _worldView;

        private readonly Stacks _stacks;
        private readonly List<StackView> _stackViews;
        private readonly InputHandler _input;

        private Queue<StackView> _ordersQueue;
        private readonly List<Guid> _selectedThisTurn;

        internal Texture2D GuiTextures { get; private set; }
        internal AtlasFrame SquareGreenFrame { get; private set; }
        internal AtlasFrame SquareGrayFrame { get; private set; }
        internal Texture2D UnitTextures { get; private set; }
        internal AtlasSpec2 UnitAtlas { get; private set; }

        internal StackView Current { get; private set; }

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;
        #endregion

        public int Count => _stackViews.Count;

        public StackView this[int index] => _stackViews[index];

        internal StackViews(WorldView worldView, Stacks stacks, InputHandler input)
        {
            _worldView = worldView;
            _stacks = stacks;
            _stackViews = new List<StackView>();
            Current = null;
            _ordersQueue = new Queue<StackView>();
            _selectedThisTurn = new List<Guid>();
            _input = input;

            SetupViewport(0, 0, 1670, 1080);
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            _viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            _viewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        internal void LoadContent(ContentManager content)
        {
            var atlas = AssetsManager.Instance.GetAtlas("Squares");
            GuiTextures = AssetsManager.Instance.GetTexture("Squares");
            SquareGreenFrame = atlas.Frames["SquareGreen"];
            SquareGrayFrame = atlas.Frames["SquareGray"];

            UnitAtlas = AssetsManager.Instance.GetAtlas("Units");
            UnitTextures = AssetsManager.Instance.GetTexture("Units");

            foreach (var stack in _stacks)
            {
                CreateNewStackView(_worldView, stack, _input);
            }
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            while (_stackViews.Count < _stacks.Count)
            {
                CreateNewStackView(_worldView, _stacks[_stackViews.Count], input);
            }

            foreach (var stackView in _stackViews)
            {
                stackView.Update(input, deltaTime);
            }
        }

        internal void RemoveDeadUnits()
        {
            var numberRemoved = _stackViews.RemoveAll(stackView => stackView.Count == 0);

            if (numberRemoved > 0)
            {
                _stacks.RemoveDeadUnits();
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform * _viewportAdapter.GetScaleMatrix()); // FrontToBack

            foreach (var stackView in _stackViews)
            {
                stackView.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        internal void BeginTurn()
        {
            // create a queue of stacks that need orders
            var queue = new Queue<StackView>();
            foreach (var stackView in _stackViews)
            {
                if (!stackView.IsBusy) // not patrol, or fortify
                {
                    queue.Enqueue(stackView);
                }
            }

            _ordersQueue = queue;
            _selectedThisTurn.Clear();

            SelectNext();
        }

        internal void DoWaitAction()
        {
            // send current to back of the queue
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

        internal void DoFortifyAction()
        {
            Current.DoFortifyAction();
            SelectNext();
        }

        internal void DoExploreAction()
        {
            Current.DoExploreAction();
        }

        internal void DoBuildAction()
        {
            Current.DoBuildAction();
            SelectNext();
        }

        internal void SetCurrent(StackView stackView)
        {
            _selectedThisTurn.Add(stackView.Id);
            if (Current != null)
            {
                _ordersQueue.Enqueue(Current);
            }

            Current = stackView;
            Current.SetStatusToNone();
        }

        internal void  SelectNext()
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
                    _worldView.Camera.LookAtCell(Current.Location);
                }
            }
            else
            {
                //_worldView.EndTurn(); // auto end turn when no stacks have actions
                Current = null;
            }
        }

        private void CreateNewStackView(WorldView worldView, PhoenixGameLibrary.Stack stack, InputHandler input)
        {
            var stackView = new StackView(worldView, this, stack, input);
            _stackViews.Add(stackView);
        }

        public IEnumerator<StackView> GetEnumerator()
        {
            foreach (var item in _stackViews)
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

        private string DebuggerDisplay => $"{{Count={_stackViews.Count}}}";
    }
}