using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GuiControls;
using Input;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackView : ViewBase, IDisposable
    {
        #region State
        internal StackViews StackViews { get; }
        internal Stack Stack { get; }

        internal List<PointI> MovementPath { get; set; }

        internal StackViewState StackViewState { get; private set; }

        private StackViewUpdateActions StackViewUpdateActions { get; set; }
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

            IfThenElseProcessor = new IfThenElseProcessor();
            //IfThenElseProcessor.Add($"StackView:{Id}", 2, this, MustFindNewExploreLocation, SetMovementPathToNewExploreLocation);

            StackViewState = new StackViewNormalState(this);
            StackViewUpdateActions = StackViewUpdateActions.None;
        }

        #region Accessors
        internal bool NeedsOrders => Stack.NeedsOrders;

        internal bool IsSelected => StackViews.Current == this;

        internal EnumerableList<string> Actions => Stack.Actions;

        internal PointI LocationHex => Stack.LocationHex;
        internal float MovementPoints => Stack.MovementPoints;
        internal int SightRange => Stack.SightRange;
        internal bool IsMovingState => StackViewState is StackViewMovingState;
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

        internal void FocusCameraOn()
        {
            WorldView.Camera.LookAtCell(LocationHex);
        }

        internal void Update(float deltaTime)
        {
            IfThenElseProcessor.Update(deltaTime);

            if (Stack.Status == UnitStatus.Explore && HasNoMovementPath)
            {
                StackViewState = new StackViewExploringState(this);
            }

            var changeState = StackViewState.Update(StackViewUpdateActions, WorldView, deltaTime);
            if (changeState.changeState)
            {
                StackViewState = changeState.stateToChangeTo;
            }
            
            StackViewUpdateActions = StackViewUpdateActions.None;
        }

        internal void DoAction(string action)
        {
            Stack.DoAction(action);
        }

        internal void SetStatusToNone()
        {
            Stack.SetStatusToNone();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            StackViewState.DrawUnit(spriteBatch, WorldView.Camera);
        }

        internal void SetPotentialMovement(Point mouseLocation)
        {
            var stackViewSelectedState = StackViewState as StackViewSelectedState;
            stackViewSelectedState.SetPotentialMovementPath(WorldView.CellGrid, WorldView.Camera, mouseLocation);
        }

        internal void ResetPotentialMovement()
        {
            var stackViewSelectedState = StackViewState as StackViewSelectedState;
            stackViewSelectedState.ResetPotentialMovementPath();
        }

        internal void CheckForUnitMovementFromMouseInitiation(Point mouseLocation)
        {
            var stackViewSelectedState = StackViewState as StackViewSelectedState;
            var changeState = stackViewSelectedState.CheckForUnitMovementFromMouseInitiation(WorldView.CellGrid, WorldView.Camera, mouseLocation);

            if (changeState.changeState)
            {
                StackViewState = changeState.stateToChangeTo;
            }
        }

        internal void CheckForUnitMovementFromKeyboardInitiation(Keys key)
        {
            var stackViewSelectedState = StackViewState as StackViewSelectedState;
            var changeState = stackViewSelectedState.CheckForUnitMovementFromKeyboardInitiation(WorldView.CellGrid, key);

            if (changeState.changeState)
            {
                StackViewState = changeState.stateToChangeTo;
            }
        }

        internal void Select()
        {
            StackViewUpdateActions = StackViewUpdateActions | StackViewUpdateActions.SelectStackDirect;
        }

        internal void Unselect()
        {
            StackViewUpdateActions = StackViewUpdateActions | StackViewUpdateActions.UnselectStackDirect;
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