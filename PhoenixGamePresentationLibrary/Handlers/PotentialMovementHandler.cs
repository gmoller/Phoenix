using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Input;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary.Views;
using Utilities;

namespace PhoenixGamePresentationLibrary.Handlers
{
    internal static class PotentialMovementHandler
    {
        internal static bool MustDeterminePotentialMovementPath(InputHandler input, StackView stackView)
        {
            if (stackView.Status == UnitStatus.Explore || stackView.IsMovingState || !input.MouseIsWithinScreen || !input.IsRightMouseButtonDown) return false;

            return true;
        }

        internal static List<Point> GetPotentialMovementPath(StackView stackView, World world)
        {
            var (potentialMovement, hexToMoveTo) = CheckForPotentialUnitMovement(stackView, world);
            if (!potentialMovement) return new List<Point>();

            var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, hexToMoveTo, world);

            return path;

        }

        private static (bool potentialMovement, Point hexToMoveTo) CheckForPotentialUnitMovement(StackView stackView, World world)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var hexToMoveTo = context.WorldHexPointedAtByMouseCursor;
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new Point(0, 0));
            var costToMoveIntoResult = stackView.GetCostToMoveInto(cellToMoveTo);

            return (costToMoveIntoResult.CanMoveInto, hexToMoveTo);
        }
    }
}