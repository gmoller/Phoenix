using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Zen.GuiControls;
using Zen.Hexagons;
using Zen.Input;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackView : ViewBase, IDisposable
    {
        #region State
        private readonly Point _frameSize = new Point(60, 60);
        internal StackViews StackViews { get; }
        internal Stack Stack { get; }

        internal List<PointI> MovementPath { get; set; }
        internal List<PointI> PotentialMovementPath { get; set; }

        private StackViewState StackViewState { get; set; }
        private StackViewStateMachine StateMachine { get; }

        #endregion

        internal StackView(WorldView worldView, StackViews stackViews, Stack stack, InputHandler input)
        {
            Id = stackViews.GetNextId();
            WorldView = worldView;
            StackViews = stackViews;
            Stack = stack;

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), $"StackView:{Id}");
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), $"StackView:{Id}");

            WorldView.SubscribeToStatusChanges($"StackView:{Id}", WorldView.HandleStatusChange);

            StackViewState = new StackViewNormalState(this);
            StateMachine = new StackViewStateMachine();
        }

        #region Accessors
        internal HexLibrary HexLibrary => WorldView.HexLibrary;
        internal CellGrid CellGrid => WorldView.CellGrid;
        internal bool OrdersGiven => Stack.OrdersGiven;

        internal bool IsSelected => StackViews.Current == this;

        internal EnumerableList<string> Actions => Stack.Actions;

        internal PointI LocationHex => Stack.LocationHex;
        internal float MovementPoints => Stack.MovementPoints;
        internal int SightRange => Stack.SightRange;
        internal bool IsMovingState => StackViewState is StackViewMovingState;
        public bool StackHasMovementPoints => Stack.MovementPoints > 0.0f;
        public bool StackHasNoMovementPoints => Stack.MovementPoints <= 0.0f;
        internal bool HasMovementPath => MovementPath?.Count > 0;

        internal bool HasNoMovementPath
        {
            get
            {
                if (MovementPath != null)
                {
                    return MovementPath.Count == 0;
                }

                return true;
            }
        }

        internal Camera Camera => WorldView.Camera;
        internal Point MousePosition => Input.MousePosition;
        internal Rectangle WorldFrame
        {
            get
            {
                var locationInWorld = WorldView.Camera.WorldHexToWorldPixel(Stack.LocationHex);
                var frame = MakeRectangle(locationInWorld);

                return frame;
            }
        }

        internal Rectangle ScreenFrame
        {
            get
            {
                var screenPixel = WorldView.Camera.WorldHexToScreenPixel(Stack.LocationHex);
                var frame = MakeRectangle(screenPixel);

                return frame;
            }
        }

        private Rectangle MakeRectangle(Vector2 pos)
        {
            var rectangle = new Rectangle((int)(pos.X - _frameSize.X * Constants.ONE_HALF), (int)(pos.Y - _frameSize.Y * Constants.ONE_HALF), _frameSize.X, _frameSize.Y);

            return rectangle;
        }

        internal int Count => Stack.Count;
        #endregion

        internal List<IControl> GetMovementTypeImages()
        {
            // TODO: update will not be called on these
            var imgMovementTypes = new List<IControl>();
            foreach (var movementType in Stack.MovementTypes)
            {
                var img = WorldView.GetMovementTypeImages[movementType];
                imgMovementTypes.Add(img);
            }

            return imgMovementTypes;
        }

        internal void SetStackViewState(StackViewState state)
        {
            StackViewState = state;
        }

        internal void SetAsCurrent(StackView stackView)
        {
            StackViews.SetCurrent(stackView);
        }

        internal void SetNotCurrent()
        {
            StackViews.SetNotCurrent();
        }

        internal void FocusCameraOn()
        {
            WorldView.Camera.LookAtCell(LocationHex);
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Update(float deltaTime)
        {
            StackViewState.Update(WorldView, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            StackViewState.DrawUnit(spriteBatch, WorldView.Camera);
        }

        internal void SelectNext()
        {
            StackViews.SelectNext();
        }

        internal void DoAction(string action)
        {
            Stack.DoAction(action);
            //TODO: de-hardcode
            switch (action)
            {
                case "Patrol":
                    StateMachine.Patrol(this);
                    break;
                case "Fortify":
                    StateMachine.Fortify(this);
                    break;
                case "Explore":
                    StateMachine.Explore(this, WorldView);
                    break;
            }
        }

        internal void SetStatusToNone()
        {
            Stack.SetStatusToNone();
        }

        internal void SetPotentialMovement()
        {
            StateMachine.ShowPotentialMovement(this);
        }

        internal void ResetPotentialMovement()
        {
            StateMachine.ResetPotentialMovement(this);
        }

        internal void CheckForUnitMovementFromMouseInitiation(Point mouseLocation)
        {
            MovementHandler.HexLibrary = HexLibrary;
            MovementHandler.CheckForUnitMovement(MovementHandler.CheckForUnitMovementFromMouseInitiation, Stack, (WorldView.CellGrid, mouseLocation, WorldView.Camera), this);
        }

        internal void CheckForUnitMovementFromKeyboardInitiation(Keys key)
        {
            MovementHandler.HexLibrary = HexLibrary;
            MovementHandler.CheckForUnitMovement(MovementHandler.CheckForUnitMovementFromKeyboardInitiation, Stack, key, this);
        }

        internal void Select()
        {
            StateMachine.Select(this);

            if (Stack.Status == UnitStatus.Explore)
            {
                StateMachine.Explore(this, WorldView);
            }
        }

        internal void Unselect()
        {
            StateMachine.Unselect(this);
        }

        internal void Move()
        {
            StateMachine.Move(this);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        } 

        private string DebuggerDisplay => $"{{Id={Id},UnitsInStack={Stack.Count}}}";

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler($"StackView:{Id}");

                // set large fields to null
                StackViewState = null;

                IsDisposed = true;
            }
        }
    }
}