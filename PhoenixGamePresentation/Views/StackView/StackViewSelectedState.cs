using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hex;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Utilities;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewSelectedState : StackViewState
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 250.0f;

        #region State
        private float BlinkCooldownInMilliseconds { get; set; }
        private bool Blink { get; set; }
        #endregion

        internal StackViewSelectedState(StackView stackView)
        {
            StackView = stackView;
            BlinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            Blink = false;
            StackView.PotentialMovementPath = new List<PointI>();
            StackView.SetAsCurrent(StackView);
            StackView.FocusCameraOn();
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            if (CheckForBlinkChange(deltaTime))
            {
                ToggleBlink();
            }

            var restartMovement = CheckForRestartOfMovement();

            if (restartMovement)
            {
                StackView.Move();
            }
        }

        private void ToggleBlink()
        {
            Blink = !Blink;
            BlinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
        }

        private bool CheckForRestartOfMovement()
        {
            if (StackView.HasMovementPath && StackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            if (!Blink)
            {
                var locationInWorld = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);
                DrawUnit(spriteBatch, locationInWorld);
            }
        }

        internal void CheckForUnitMovementFromMouseInitiation(CellGrid cellGrid, Camera camera, Point mouseLocation)
        {
            var mustStartMovement = CheckForUnitMovementFromMouseInitiation(StackView.Stack, cellGrid, mouseLocation, camera);

            if (mustStartMovement.startMovement)
            {
                var path = MovementPathDeterminer.DetermineMovementPath(StackView.Stack, StackView.LocationHex, mustStartMovement.hexToMoveTo, StackView.CellGrid);
                StackView.MovementPath = path;
                StackView.Move();
            }
        }

        internal void CheckForUnitMovementFromKeyboardInitiation(Keys key)
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
                    var path = MovementPathDeterminer.DetermineMovementPath(StackView.Stack, StackView.LocationHex, hexToMoveTo, StackView.CellGrid);
                    StackView.MovementPath = path;
                    StackView.Move();
                }
            }
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "Selected";
    }
}