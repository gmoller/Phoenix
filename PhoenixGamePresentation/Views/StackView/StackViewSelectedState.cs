using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hex;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Utilities;

namespace PhoenixGamePresentation.Views.StackView
{
    internal class StackViewSelectedState : StackViewState
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 250.0f;

        #region State
        private float BlinkCooldownInMilliseconds { get; set; }
        private bool Blink { get; set; }
        private List<PointI> PotentialMovementPath { get; set; }
        #endregion End State

        internal StackViewSelectedState(StackView stackView)
        {
            StackView = stackView;
            BlinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            Blink = false;
            PotentialMovementPath = new List<PointI>();
            StackView.StackViews.SetCurrent(StackView);
        }

        internal override (bool changeState, StackViewState stateToChangeTo) Update(StackViewUpdateActions updateActions, WorldView worldView, float deltaTime)
        {
            if (updateActions.HasFlag(StackViewUpdateActions.UnselectStackDirect))
            {
                return (true, new StackViewNormalState(StackView));
            }

            if (CheckForBlinkChange(deltaTime))
            {
                Blink = !Blink;
                BlinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            }

            var restartMovement = CheckForRestartOfMovement();

            if (restartMovement)
            {
                return (true, new StackViewMovingState(StackView.MovementPath, StackView));
            }

            return (false, null);
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            if (!Blink)
            {
                var location = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);

                DrawUnit(spriteBatch, location);
            }

            DrawMovementPath(spriteBatch, PotentialMovementPath, Color.White, 3.0f, 1.0f);
        }

        internal void SetPotentialMovementPath(CellGrid cellGrid, Camera camera, Point mouseLocation)
        {
            var path = PotentialMovementHandler.GetPotentialMovementPath(StackView.Stack, cellGrid, mouseLocation, camera);
            PotentialMovementPath = path;
        }

        internal void ResetPotentialMovementPath()
        {
            PotentialMovementPath = new List<PointI>();
        }

        internal (bool changeState, StackViewState stateToChangeTo) CheckForUnitMovementFromMouseInitiation(CellGrid cellGrid, Camera camera, Point mouseLocation)
        {
            var mustStartMovement = CheckForUnitMovementFromMouseInitiation(StackView.Stack, cellGrid, mouseLocation, camera);

            if (mustStartMovement.startMovement)
            {
                return (true, new StackViewMovingState(mustStartMovement.hexToMoveTo, cellGrid, StackView));
            }

            return (false, null);
        }

        internal (bool changeState, StackViewState stateToChangeTo) CheckForUnitMovementFromKeyboardInitiation(CellGrid cellGrid, Keys key)
        {
            if (StackView.MovementPoints > 0.0f)
            {
                var direction = key switch
                {
                    Keys.NumPad1 => Direction.SouthWest,
                    Keys.NumPad3 => Direction.SouthEast,
                    Keys.NumPad4 => Direction.West,
                    Keys.NumPad6 => Direction.East,
                    Keys.NumPad7 => Direction.NorthWest,
                    Keys.NumPad9 => Direction.NorthEast,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var neighbor = HexOffsetCoordinates.GetNeighbor(StackView.LocationHex, direction);
                var hexToMoveTo = neighbor.ToPointI();

                var costToMoveIntoResult = StackView.Stack.GetCostToMoveInto(hexToMoveTo);

                if (costToMoveIntoResult.CanMoveInto)
                {
                    return (true, new StackViewMovingState(hexToMoveTo, cellGrid, StackView));
                }
            }

            return (false, null);
        }

        private (bool startMovement, PointI hexToMoveTo) CheckForUnitMovementFromMouseInitiation(Stack stack, CellGrid cellGrid, Point mouseLocation, Camera camera)
        {
            if (!camera.GetViewport.Contains(mouseLocation)) return (false, new PointI(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = camera.ScreenPixelToWorldHex(mouseLocation).ToPointI();
            if (hexToMoveTo.Equals(stack.LocationHex)) return (false, new PointI(0, 0));
            var cellToMoveTo = cellGrid.GetCell(hexToMoveTo);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new PointI(0, 0));

            var costToMoveIntoResult = stack.GetCostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new PointI(0, 0));
        }

        private bool CheckForBlinkChange(float deltaTime)
        {
            BlinkCooldownInMilliseconds -= deltaTime;
            if (BlinkCooldownInMilliseconds > 0.0f) return false;

            return true;
        }

        private bool CheckForRestartOfMovement()
        {
            if (StackView.HasMovementPath && StackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }
    }
}