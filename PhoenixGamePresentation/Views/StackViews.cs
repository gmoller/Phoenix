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
        internal bool AllStacksHaveBeenGivenOrders => GetAllStacksHaveBeenGivenOrders();
        internal StackView.StackView this[int index] => StackViewsList[index];
        #endregion

        private bool GetAllStacksHaveBeenGivenOrders()
        {
            return Stacks.All(stack => stack.OrdersGiven);
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
            Current = null;
            SelectFirstThatHasNoOrders();
        }

        private void SelectFirstThatHasNoOrders()
        {
            foreach (var stackView in this)
            {
                if (!stackView.OrdersGiven)
                {
                    Current = stackView;
                    Current.Select();
                    break;
                }
            }
        }

        internal void SelectNext()
        {
            if (Current is null)
            {
                SelectFirstThatHasNoOrders();
            }
            else
            {
                var id = Current.Id;
                Current.Unselect();
                Current = null;
                var oldCurrentIndex = -1;
                for (var i = 0; i < Count; i++)
                {
                    var stackView = this[i];
                    if (stackView.Id == id)
                    {
                        oldCurrentIndex = i;
                        break;
                    }
                }

                if (oldCurrentIndex >= 0)
                {
                    if (oldCurrentIndex >= Count - 1)
                    {
                        SelectFirstThatHasNoOrders();
                    }
                    else
                    {
                        Current = this[oldCurrentIndex + 1];
                        if (Current.OrdersGiven)
                        {
                            SelectNext();
                        }
                        else
                        {
                            Current.Select();
                        }
                    }
                }
                else
                {
                    throw new Exception("Strange...");
                }
            }
        }

        internal void SetCurrent(StackView.StackView stackView)
        {
            Current = stackView;
        }

        internal void SetNotCurrent()
        {
            Current = null;
        }

        internal void DoAction(string action)
        {
            //TODO: de-hardcode
            if (action == "Done" || action == "Patrol" || action == "Fortify" || action == "Explore" || action == "BuildOutpost")
            {
                Current.DoAction(action);
            }

            if (action == "Done" || action == "Wait" || action == "Patrol" || action == "Fortify" || action == "BuildOutpost")
            {
                SelectNext();
            }
        }

        private void CreateNewStackView(WorldView worldView, PhoenixGameLibrary.Stack stack, InputHandler input)
        {
            var stackView = new StackView.StackView(worldView, this, stack, input);
            StackViewsList.Add(stackView);
        }

        internal StackView.StackView GetStackViewFromLocation(Point mouseLocation)
        {
            return this.FirstOrDefault(stackView => mouseLocation.IsWithinRectangle(stackView.ScreenFrame));
        }

        internal void CheckForSelectionOfStack(Point mouseLocation)
        {
            foreach (var stackView in this)
            {
                var mustSelect = stackView.MovementPoints > 0.0f && mouseLocation.IsWithinHex(stackView.LocationHex, WorldView.Camera.Transform, WorldView.HexLibrary);
                if (mustSelect)
                {
                    if (stackView.Id != Current?.Id)
                    {
                        Current?.Unselect();

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