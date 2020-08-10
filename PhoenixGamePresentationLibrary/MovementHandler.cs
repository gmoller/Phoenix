using System;
using Microsoft.Xna.Framework.Input;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    internal static class MovementHandler
    {
        internal static void HandleMovement(InputHandler input, StackView stackView, float deltaTime, Action restartUnitMovement, Action<Point> startUnitMovement, Action<float> moveUnit, Action moveUnitToCell)
        {
            var restartMovement = CheckForRestartOfMovement(stackView);
            if (restartMovement)
            {
                restartUnitMovement();
            }

            var startUnitMovementKeyboard = CheckForUnitMovementFromKeyboardInitiation(input, stackView);
            var startUnitMovementMouse = CheckForUnitMovementFromMouseInitiation(input, stackView);

            if (startUnitMovementKeyboard.startMovement || startUnitMovementMouse.startMovement)
            {
                startUnitMovement(startUnitMovementKeyboard.startMovement ? startUnitMovementKeyboard.hexToMoveTo : startUnitMovementMouse.hexToMoveTo);
            }

            if (UnitIsMoving(stackView))
            {
                moveUnit(deltaTime);
                var unitHasReachedNextCell = CheckIfUnitHasReachedNextCell(stackView);
                if (unitHasReachedNextCell)
                {
                    moveUnitToCell();
                }
            }
        }

        private static bool CheckForRestartOfMovement(StackView stackView)
        {
            if (stackView.IsMovingState == false && stackView.MovementPath.Count > 0 && stackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        private static (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromKeyboardInitiation(InputHandler input, StackView stackView)
        {
            if (stackView.IsMovingState || stackView.MovementPoints.AboutEquals(0.0f) || !input.AreAnyNumPadKeysDown) return (false, new Point(0, 0));

            var direction = DetermineDirection(input);
            if (direction == Direction.None) return (false, new Point(0, 0));

            var neighbor = HexOffsetCoordinates.GetNeighbor(stackView.Location.X, stackView.Location.Y, direction);
            var hexToMoveTo = new Point(neighbor.Col, neighbor.Row);

            var costToMoveIntoResult = stackView.FirstUnit.CostToMoveInto(hexToMoveTo); // TODO: fix this, make work on stack

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private static (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromMouseInitiation(InputHandler input, StackView stackView)
        {
            if (stackView.IsMovingState || stackView.MovementPoints.AboutEquals(0.0f) || !input.IsLeftMouseButtonReleased || input.Eaten) return (false, new Point(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            if (hexToMoveTo == stackView.FirstUnit.Location) return (false, new Point(0, 0));
            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new Point(0, 0));

            var costToMoveIntoResult = stackView.FirstUnit.CostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private static Direction DetermineDirection(InputHandler input)
        {
            if (input.IsKeyDown(Keys.NumPad4))
            {
                return Direction.West;
            }

            if (input.IsKeyDown(Keys.NumPad6))
            {
                return Direction.East;
            }

            if (input.IsKeyDown(Keys.NumPad7))
            {
                return Direction.NorthWest;
            }

            if (input.IsKeyDown(Keys.NumPad9))
            {
                return Direction.NorthEast;
            }

            if (input.IsKeyDown(Keys.NumPad1))
            {
                return Direction.SouthWest;
            }

            if (input.IsKeyDown(Keys.NumPad3))
            {
                return Direction.SouthEast;
            }

            return Direction.None;
        }

        private static bool UnitIsMoving(StackView stackView)
        {
            return stackView.IsMovingState;
        }

        private static bool CheckIfUnitHasReachedNextCell(StackView stackView)
        {
            return stackView.MovementCountdownTime <= 0;
        }
    }
}