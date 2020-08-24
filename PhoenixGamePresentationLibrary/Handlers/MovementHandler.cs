using System;
using System.Runtime.Remoting.Messaging;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework.Input;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary.Views;
using Utilities.ExtensionMethods;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary.Handlers
{
    internal static class MovementHandler
    {
        internal static bool CheckForRestartOfMovement(StackView stackView)
        {
            if (stackView.IsMovingState == false && stackView.MovementPath.Count > 0 && stackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        internal static bool CheckForStartOfMovement(InputHandler input, StackView stackView, World world)
        {
            var (startMovementKeyboard, hexToMoveToKeyboard) = CheckForUnitMovementFromKeyboardInitiation(input, stackView);
            var (startMovementMouse, hexToMoveToMouse) = CheckForUnitMovementFromMouseInitiation(input, stackView, world);

            if (startMovementKeyboard || startMovementMouse)
            {
                return true;
            }

            return false;
        }

        internal static bool MustContinueMovement(StackView stackView)
        {
            return UnitIsMoving(stackView);
        }

        internal static bool MustMoveUnitToNextCell(StackView stackView)
        {
            return UnitIsMoving(stackView) && CheckIfUnitHasReachedNextCell(stackView);
        }

        internal static Point GetHexToMoveTo(InputHandler input, StackView stackView, World world)
        {
            var (startMovementKeyboard, hexToMoveToKeyboard) = CheckForUnitMovementFromKeyboardInitiation(input, stackView);
            var (startMovementMouse, hexToMoveToMouse) = CheckForUnitMovementFromMouseInitiation(input, stackView, world);

            return startMovementKeyboard ? hexToMoveToKeyboard : hexToMoveToMouse;
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