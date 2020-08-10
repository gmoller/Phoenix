using System;
using System.Collections.Generic;
using Input;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal static class PotentialMovementHandler
    {
        internal static void HandlePotentialMovement(InputHandler input, StackView stackView, World world, Action<List<Point>> action)
        {
            if (stackView.Status == UnitStatus.Explore || stackView.IsMovingState || !input.MouseIsWithinScreen || input.Eaten) return;

            var path = GetPotentialMovementPath(stackView, world);

            action(path);
        }

        private static List<Point> GetPotentialMovementPath(StackView stackView, World world)
        {
            var (potentialMovement, hexToMoveTo) = CheckForPotentialUnitMovement(stackView, world);
            if (!potentialMovement) return new List<Point>();

            var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, hexToMoveTo);

            return path;

        }

        private static (bool potentialMovement, Point hexToMoveTo) CheckForPotentialUnitMovement(StackView stackView, World world)
        {
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new Point(0, 0));
            var costToMoveIntoResult = stackView.GetCostToMoveInto(cellToMoveTo);

            return (costToMoveIntoResult.CanMoveInto, hexToMoveTo);
        }
    }
}