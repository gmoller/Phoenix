using System.Collections.Generic;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal static class MovementPathDeterminer
    {
        internal static List<Point> DetermineMovementPath(StackView stackView, Point from, Point to)
        {
            if (from.Equals(to)) return new List<Point>();

            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            mapSolver.Solve(GetCostToMoveIntoFunc, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), from, to, openList, closedList);

            var path = mapSolver.Solution;

            return path;

            GetCostToMoveIntoResult GetCostToMoveIntoFunc(Point point)
            {
                return stackView.GetCostToMoveInto(point);
            }
        }
    }
}