using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Input;
using Microsoft.Xna.Framework;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;

namespace PhoenixGamePresentation.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViews : ViewBase, IEnumerable<StackView.StackView>, IDisposable
    {
        #region State
        private long NextId { get; set; }

        private Stacks Stacks { get; }
        private List<StackView.StackView> StackViewsList { get; }

        private Queue<StackView.StackView> OrdersQueue { get; set; }

        internal Texture2D GuiTextures { get; private set; }
        internal AtlasFrame SquareGreenFrame { get; private set; }
        internal AtlasFrame SquareGrayFrame { get; private set; }
        internal Texture2D UnitTextures { get; private set; }
        internal AtlasSpec2 UnitAtlas { get; private set; }

        internal StackView.StackView Current { get; private set; }
        #endregion

        internal StackViews(WorldView worldView, Stacks stacks, InputHandler input)
        {
            WorldView = worldView;
            Stacks = stacks;
            StackViewsList = new List<StackView.StackView>();
            Current = null;
            OrdersQueue = new Queue<StackView.StackView>();
            NextId = 1;

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "StackViews");
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "StackViews");

            WorldView.SubscribeToStatusChanges("StackViews", worldView.HandleStatusChange);
        }

        #region Accessors
        internal int Count => StackViewsList.Count;
        internal string OrdersQueueList => GetOrdersQueueList();
        internal bool AllStacksHaveBeenGivenOrders => OrdersQueue.Count == 0;
        internal StackView.StackView this[int index] => StackViewsList[index];
        #endregion

        private string GetOrdersQueueList()
        {
            var array = OrdersQueue.ToArray().ToList();
            var ret = string.Join(",", array);

            return ret;
        }

        internal long GetNextId()
        {
            var nextId = NextId;
            NextId++;

            return nextId;
        }

        internal void LoadContent(ContentManager content)
        {
            var atlas = AssetsManager.Instance.GetAtlas("Squares");
            GuiTextures = AssetsManager.Instance.GetTexture("Squares");
            SquareGreenFrame = atlas.Frames["SquareGreen"];
            SquareGrayFrame = atlas.Frames["SquareGray"];

            UnitAtlas = AssetsManager.Instance.GetAtlas("Units");
            UnitTextures = AssetsManager.Instance.GetTexture("Units");

            foreach (var stack in Stacks)
            {
                //CreateNewStackView(WorldView, stack, Input);
                var stackView = new StackView.StackView(WorldView, this, stack, Input);
                stackView.LoadContent(content);
                StackViewsList.Add(stackView);
            }
        }

        internal void Update(float deltaTime)
        {
            while (StackViewsList.Count < Stacks.Count)
            {
                CreateNewStackView(WorldView, Stacks[StackViewsList.Count], Input);
            }

            foreach (var stackView in StackViewsList)
            {
                stackView.Update(deltaTime);
            }
        }

        internal void RemoveDeadUnits()
        {
            var numberRemoved = StackViewsList.RemoveAll(stackView => stackView.Count == 0);

            if (numberRemoved > 0)
            {
                Stacks.RemoveDeadUnits();
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform * ViewportAdapter.GetScaleMatrix()); // FrontToBack

            foreach (var stackView in StackViewsList)
            {
                stackView.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        internal void BeginTurn()
        {
            // create a queue of stacks that need orders
            var queue = new Queue<StackView.StackView>();
            foreach (var stackView in StackViewsList)
            {
                if (stackView.NeedsOrders)
                {
                    queue.Enqueue(stackView);
                }
            }

            OrdersQueue = queue;

            SelectFirst();
        }

        private void SelectFirst()
        {
            if (OrdersQueue.Count > 0)
            {
                Current = OrdersQueue.Dequeue();
                Current.Select();
            }
            else
            {
                Current = null;
            }
        }

        private void SelectNext()
        {
            Current?.Unselect();
            if (OrdersQueue.Count > 0)
            {
                Current = OrdersQueue.Dequeue();
                Current.Select();
            }
            else
            {
                //_worldView.EndTurn(); // auto end turn when no stacks have actions
                Current = null;
            }
        }

        internal void SetCurrent(StackView.StackView stackView)
        {
            if (Current != null && Current.Id != stackView.Id)
            {
                OrdersQueue.Enqueue(Current);
            }

            Current = stackView;
        }

        internal void SetNotCurrent(StackView.StackView stackView)
        {
            Current = null;
            //if (Current == stackView) // if we were the currently selected one
            //{
            //    SelectNext();
            //}
        }

        internal void DoAction(string action)
        {
            if (action == "Wait")
            {
                if (OrdersQueue.Count > 0)
                {
                    OrdersQueue.Enqueue(Current);
                }
            }

            if (action == "Patrol" || action == "Fortify" || action == "Explore" || action == "BuildOutpost")
            {
                Current.DoAction(action);
            }

            if (action == "Done" || action == "Wait" || action == "Patrol" || action == "Fortify" || action == "BuildOutpost")
            {
                if (action != "Wait" || OrdersQueue.Count != 0)
                {
                    SelectNext();
                }
            }
        }

        private void CreateNewStackView(WorldView worldView, PhoenixGameLibrary.Stack stack, InputHandler input)
        {
            var stackView = new StackView.StackView(worldView, this, stack, input);
            StackViewsList.Add(stackView);
        }

        internal StackView.StackView GetStackViewFromLocation(Point location)
        {
            foreach (var stackView in this)
            {
                if (location.IsWithinHex(stackView.LocationHex, WorldView.Camera.Transform))
                {
                    return stackView;
                }
            }

            return null;
        }

        internal void CheckForSelectionOfStack(Point mouseLocation)
        {
            foreach (var stackView in this)
            {
                var mustSelect = stackView.MovementPoints > 0.0f && mouseLocation.IsWithinHex(stackView.LocationHex, WorldView.Camera.Transform);
                if (mustSelect)
                {
                    if (stackView.Id != Current?.Id)
                    {
                        if (Current != null)
                        {
                            OrdersQueue.Enqueue(Current);
                            Current.Unselect();
                        }

                        stackView.Select();
                        Current = stackView;
                    }
                }
            }
        }

        public IEnumerator<StackView.StackView> GetEnumerator()
        {
            foreach (var item in StackViewsList)
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

        private string DebuggerDisplay => $"{{Count={StackViewsList.Count}}}";

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("StackViews");

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}