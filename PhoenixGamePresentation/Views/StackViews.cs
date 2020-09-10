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
    internal class StackViews : IEnumerable<StackView>, IDisposable
    {
        #region State
        private WorldView WorldView { get; } // readonly

        private Stacks Stacks { get; } // readonly
        private List<StackView> StackViewsList { get; } // readonly

        private Queue<StackView> OrdersQueue { get; set; }
        private List<Guid> SelectedThisTurn { get; } // readonly

        internal Texture2D GuiTextures { get; private set; }
        internal AtlasFrame SquareGreenFrame { get; private set; }
        internal AtlasFrame SquareGrayFrame { get; private set; }
        internal Texture2D UnitTextures { get; private set; }
        internal AtlasSpec2 UnitAtlas { get; private set; }

        internal StackView Current { get; private set; }

        private Viewport Viewport { get; set; }
        private ViewportAdapter ViewportAdapter { get; set; }

        private InputHandler Input { get; } // readonly
        private bool IsDisposed { get; set; }
        #endregion End State

        public int Count => StackViewsList.Count;

        public StackView this[int index] => StackViewsList[index];

        internal StackViews(WorldView worldView, Stacks stacks, InputHandler input)
        {
            WorldView = worldView;
            Stacks = stacks;
            StackViewsList = new List<StackView>();
            Current = null;
            OrdersQueue = new Queue<StackView>();
            SelectedThisTurn = new List<Guid>();

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
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
                CreateNewStackView(WorldView, stack, Input);
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
            var queue = new Queue<StackView>();
            foreach (var stackView in StackViewsList)
            {
                if (!stackView.IsBusy) // not patrol, or fortify
                {
                    queue.Enqueue(stackView);
                }
            }

            OrdersQueue = queue;
            SelectedThisTurn.Clear();

            SelectNext();
        }

        internal void DoWaitAction()
        {
            // send current to back of the queue
            OrdersQueue.Enqueue(Current);
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
            SelectedThisTurn.Add(stackView.Id);
            if (Current != null)
            {
                OrdersQueue.Enqueue(Current);
            }

            Current = stackView;
            Current.SetStatusToNone();
        }

        internal void  SelectNext()
        {
            if (OrdersQueue.Count > 0)
            {
                Current = OrdersQueue.Dequeue();
                if (SelectedThisTurn.Contains(Current.Id))
                {
                    SelectNext();
                }
                else
                {
                    WorldView.Camera.LookAtCell(Current.Location);
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
            StackViewsList.Add(stackView);
        }

        public IEnumerator<StackView> GetEnumerator()
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