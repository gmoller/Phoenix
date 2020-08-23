using System;
using System.Collections.Generic;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary.Views;
using Utilities;

namespace PhoenixGamePresentationLibrary.Handlers
{
    internal static class ExploreHandler
    {
        //private static void HandleExplore(StackView stackView, Action<List<Point>> action, World world)
        //{
        //    if (stackView.Status != UnitStatus.Explore) return;
        //    if (stackView.MovementPath.Count != 0) return;

        //    // if no destination, choose one
        //    // find closest unexplored cell
        //    var cell = world.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

        //    // find best path to unexplored cell
        //    var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, cell.ToPoint, world);

        //    if (path.Count > 0)
        //    {
        //        action(path);
        //    }
        //    else
        //    {
        //        stackView.SetStatusToNone();
        //    }
        //}

        internal static bool MustFindNewExploreLocation(StackView stackView)
        {
            var mustFindNewExploreLocation = stackView.Status == UnitStatus.Explore && stackView.HasNoMovementPath();

            return mustFindNewExploreLocation;
        }

        internal static void SetMovementPathToNewExploreLocation(StackView stackView, World world)
        {
            // find closest unexplored cell
            var cell = world.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, cell.ToPoint, world);

            if (path.Count > 0)
            {
                stackView.SetMovementPath(path);
            }
            else
            {
                // no location found to explore
                stackView.SetStatusToNone();
            }
        }
    }
}