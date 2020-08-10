using System;
using System.Collections.Generic;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal static class ExploreHandler
    {
        internal static void HandleExplore(StackView stackView, Action<List<Point>> action)
        {
            if (stackView.Status != UnitStatus.Explore) return;
            if (stackView.MovementPath.Count != 0) return;

            // if no destination, choose one
            // find closest unexplored cell
            var cell = Globals.Instance.World.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(stackView.FirstUnit, stackView.Location, cell.ToPoint); // TODO: don't use first unit, use stack as a whole

            if (path.Count > 0)
            {
                action(path);
            }
        }
    }
}