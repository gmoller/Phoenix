using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework.Input;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using Utilities;
using Utilities.ExtensionMethods;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    internal static class MovementHandler
    {
        internal static void HandleMovement(InputHandler input, StackView stackView, float deltaTime, Action restartUnitMovement, Action<Point> startUnitMovement, Action<float> moveStack, Action moveStackToCell, World world)
        {
            var restartMovement = CheckForRestartOfMovement(stackView);
            if (restartMovement)
            {
                restartUnitMovement();
            }

            var (startMovementKeyboard, hexToMoveToKeyboard) = CheckForUnitMovementFromKeyboardInitiation(input, stackView);
            var (startMovementMouse, hexToMoveToMouse) = CheckForUnitMovementFromMouseInitiation(input, stackView, world);

            if (startMovementKeyboard || startMovementMouse)
            {
                startUnitMovement(startMovementKeyboard ? hexToMoveToKeyboard : hexToMoveToMouse);
            }

            if (UnitIsMoving(stackView))
            {
                moveStack(deltaTime);
                var unitHasReachedNextCell = CheckIfUnitHasReachedNextCell(stackView);
                if (unitHasReachedNextCell)
                {
                    moveStackToCell();
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
            if (!direction.shouldMove) return (false, new Point(0, 0));

            var neighbor = HexOffsetCoordinates.GetNeighbor(stackView.Location.X, stackView.Location.Y, direction.direction);
            var hexToMoveTo = new Point(neighbor.Col, neighbor.Row);

            var costToMoveIntoResult = stackView.GetCostToMoveInto(hexToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private static (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromMouseInitiation(InputHandler input, StackView stackView, World world)
        {
            if (stackView.IsMovingState || stackView.MovementPoints.AboutEquals(0.0f) || !input.IsLeftMouseButtonReleased) return (false, new Point(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var hexToMoveTo = context.WorldHexPointedAtByMouseCursor;
            if (hexToMoveTo == stackView.Location) return (false, new Point(0, 0));
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new Point(0, 0));

            var costToMoveIntoResult = stackView.GetCostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private static (bool shouldMove, Direction direction) DetermineDirection(InputHandler input)
        {
            if (input.IsKeyDown(Keys.NumPad4))
            {
                return (true, Direction.West);
            }

            if (input.IsKeyDown(Keys.NumPad6))
            {
                return (true, Direction.East);
            }

            if (input.IsKeyDown(Keys.NumPad7))
            {
                return (true, Direction.NorthWest);
            }

            if (input.IsKeyDown(Keys.NumPad9))
            {
                return (true, Direction.NorthEast);
            }

            if (input.IsKeyDown(Keys.NumPad1))
            {
                return (true, Direction.SouthWest);
            }

            if (input.IsKeyDown(Keys.NumPad3))
            {
                return (true, Direction.SouthEast);
            }

            return (false, Direction.East);
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