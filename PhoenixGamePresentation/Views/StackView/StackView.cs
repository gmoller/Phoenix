using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GuiControls;
using Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackView : ViewBase, IDisposable
    {
        #region State
        internal StackViews StackViews { get; }
        internal Stack Stack { get; }

        internal List<PointI> MovementPath { get; set; }

        private StackViewState StackViewState { get; set; }

        private StackViewUpdateActions StackViewUpdateActions { get; set; }
        #endregion End State

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
            IfThenElseProcessor.Add($"StackView:{Id}", 2, this, MustFindNewExploreLocation, SetMovementPathToNewExploreLocation);

            StackViewState = new StackViewNormalState(this);
            StackViewUpdateActions = StackViewUpdateActions.None;
        }

        #region Accessors
        internal bool NeedsOrders => Stack.NeedsOrders;
        private UnitStatus Status => Stack.Status;

        internal bool IsSelected => StackViews.Current == this;

        internal EnumerableList<string> Actions => Stack.Actions;

        internal PointI LocationHex => Stack.LocationHex;
        internal float MovementPoints => Stack.MovementPoints;
        private int SightRange => Stack.SightRange;
        private bool IsMovingState => StackViewState is StackViewMovingState;
        internal bool HasMovementPath => MovementPath?.Count > 0;
        internal bool HasNoMovementPath => MovementPath?.Count == 0;

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

        internal void Update(float deltaTime)
        {
            IfThenElseProcessor.Update(deltaTime);
            
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
            if (IsSelected)
            {
                DrawUnit(spriteBatch);
            }
            else
            {
                if (StackViews.Current != null) // if another stack is selected
                {
                    var selectedStacksPosition = StackViews.Current.LocationHex;
                    var thisStacksPosition = LocationHex;
                    if (selectedStacksPosition == thisStacksPosition) // and it's in the same hex as this one
                    {
                        if (StackViews.Current.IsMovingState) // and selected stack is moving
                        {
                            DrawUnit(spriteBatch);
                        }
                        else
                        {
                            // don't draw if there's a selected stack on same location and it's not moving
                        }
                    }
                    else
                    {
                        DrawUnit(spriteBatch);
                    }
                }
                else
                {
                    DrawUnit(spriteBatch);
                }
            }
        }

        private void DrawUnit(SpriteBatch spriteBatch)
        {
            StackViewState.DrawUnit(spriteBatch, WorldView.Camera);
        }

        internal List<StackView> GetStackViewsSharingSameLocation()
        {
            var stackViews = new List<StackView>();
            foreach (var stackView in StackViews)
            {
                if (stackView.LocationHex == LocationHex) // same location
                {
                    stackViews.Add(stackView);
                }
            }

            return stackViews;
        }

        internal void DrawBadges(SpriteBatch spriteBatch, Vector2 topLeftPosition, int index = 0, bool isSelected = true)
        {
            var x = topLeftPosition.X + 60.0f * Constants.ONE_HALF;
            var y = topLeftPosition.Y + 60.0f * Constants.ONE_HALF;
            foreach (var unit in Stack)
            {
                var indexMod3 = index % 3;
                var indexDividedBy3 = index / 3; // Floor
                var xOffset = 75.0f * indexMod3;
                var yOffset = 75.0f * indexDividedBy3;
                DrawBadge(spriteBatch, new Vector2(x + xOffset, y + yOffset), unit, isSelected);
                index++;
            }
        }

        private void DrawBadge(SpriteBatch spriteBatch, Vector2 centerPosition, Unit unit, bool isSelected)
        {
            // draw background
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 60, 60);
            var sourceRectangle = isSelected ? StackViews.SquareGreenFrame.ToRectangle() : StackViews.SquareGrayFrame.ToRectangle();
            spriteBatch.Draw(StackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.FlipVertically, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 36, 32);
            var frame = StackViews.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(StackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.None, 0.0f);
        }

        private static bool MustFindNewExploreLocation(object sender, float deltaTime)
        {
            var stackView = (StackView)sender;

            var mustFindNewExploreLocation = stackView.Status == UnitStatus.Explore && stackView.HasNoMovementPath;

            return mustFindNewExploreLocation;
        }

        private static void SetMovementPathToNewExploreLocation(object sender, ActionArgs e)
        {
            var stackView = (StackView)sender;

            // find closest unexplored cell
            var cellGrid = stackView.WorldView.CellGrid;
            var cell = cellGrid.GetClosestUnexploredCell(stackView.LocationHex);

            if (cell != Cell.Empty)
            {
                // find best path to unexplored cell
                var path = MovementPathDeterminer.DetermineMovementPath(stackView.Stack, stackView.LocationHex, cell.ToPoint, cellGrid);

                if (path.Count > 0)
                {
                    path = path.RemoveLast(stackView.SightRange);
                    stackView.SetMovementPath(path);
                }
                else
                {
                    // no location found to explore
                    stackView.SetStatusToNone();
                }
            }
            else
            {
                // all locations explored
                stackView.SetStatusToNone();
            }
        }

        private void SetMovementPath(List<PointI> path)
        {
            MovementPath = path;
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