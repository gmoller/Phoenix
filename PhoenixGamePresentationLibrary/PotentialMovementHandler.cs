using System.Collections.Generic;
using Input;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class PotentialMovementHandler
    {
        internal void HandleMovement(InputHandler input, StackView stackView, World world)
        {
            if (!stackView.IsSelected || stackView.IsMovingState || !input.MouseIsWithinScreen || input.Eaten)
            {
                stackView.ResetPotentialMovementPath();
            }
            else
            {
                var potentialMovementHandler = new PotentialMovementHandler();
                var path = potentialMovementHandler.GetPotentialMovementPath(stackView, world);
                stackView.SetPotentialMovementPath(path);
            }
        }

        private List<Point> GetPotentialMovementPath(StackView stackView, World world)
        {
            var (potentialMovement, hexToMoveTo) = CheckForPotentialUnitMovement(stackView.FirstUnit, world); // TODO: first unit always used, need to check all units (assumes only 1 unit right now)
            if (potentialMovement)
            {
                var path = MovementPathDeterminer.DetermineMovementPath(stackView.FirstUnit, stackView.Location, hexToMoveTo);
                return path;
            }

            return new List<Point>();
        }

        private (bool potentialMovement, Point hexToMoveTo) CheckForPotentialUnitMovement(Unit unit, World world)
        {
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new Point(0, 0));
            var costToMoveIntoResult = unit.CostToMoveInto(cellToMoveTo);

            return (costToMoveIntoResult.CanMoveInto, hexToMoveTo);
        }
    }
}