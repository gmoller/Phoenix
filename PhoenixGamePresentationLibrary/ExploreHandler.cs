using System;
using System.Collections.Generic;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal static class ExploreHandler
    {
        internal static void HandleExplore(StackView stackView, Action<List<Point>> action, World world)
        {
            if (stackView.Status != UnitStatus.Explore) return;
            if (stackView.MovementPath.Count != 0) return;

            // if no destination, choose one
            // find closest unexplored cell
            var cell = world.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, cell.ToPoint, world);

            if (path.Count > 0)
            {
                action(path);
            }
            else
            {
                stackView.SetStatusToNone();
            }
        }
    }
}