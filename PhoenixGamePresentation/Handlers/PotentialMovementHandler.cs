using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class PotentialMovementHandler
    {
        internal static List<PointI> GetPotentialMovementPath(Stack stack, CellGrid cellGrid, Point mouseLocation, Camera camera)
        {
            var (potentialMovement, hexToMoveTo) = CheckForPotentialUnitMovement(stack, cellGrid, mouseLocation, camera);
            if (!potentialMovement) return new List<PointI>();

            var path = MovementPathDeterminer.DetermineMovementPath(stack, stack.LocationHex, hexToMoveTo, cellGrid);

            return path;

        }

        private static (bool potentialMovement, PointI hexToMoveTo) CheckForPotentialUnitMovement(Stack stack, CellGrid cellGrid, Point mouseLocation, Camera camera)
        {
            var hexToMoveTo = camera.ScreenPixelToWorldHex(mouseLocation);
            var cellToMoveTo = cellGrid.GetCell(hexToMoveTo.Col, hexToMoveTo.Row);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new PointI(0, 0));
            var costToMoveIntoResult = stack.GetCostToMoveInto(cellToMoveTo);

            return (costToMoveIntoResult.CanMoveInto, hexToMoveTo.ToPointI());
        }
    }
}