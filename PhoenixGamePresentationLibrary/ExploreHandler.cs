using System.Collections.Generic;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class ExploreHandler
    {
        internal List<Point> HandleExplore(StackView stackView)
        {
            if (stackView.Status != UnitStatus.Explore) return new List<Point>();
            if (stackView.MovementPath.Count != 0) return new List<Point>();

            // if no destination, choose one
            // find closest unexplored cell
            var cell = Globals.Instance.World.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(stackView.FirstUnit, stackView.Location, cell.ToPoint); // TODO: don't use first unit, use stack as a whole

            return path;
        }
    }
}