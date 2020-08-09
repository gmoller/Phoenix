using System.Collections.Generic;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal static class MovementPathDeterminer
    {
        internal static List<Point> DetermineMovementPath(Unit unit, Point from, Point to)
        {
            if (from.Equals(to)) return new List<Point>();

            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            mapSolver.Solve(GetCostToMoveIntoFunc, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), from, to, openList, closedList);

            var path = mapSolver.Solution;

            return path;

            CostToMoveIntoResult GetCostToMoveIntoFunc(Point point)
            {
                return unit.CostToMoveInto(point);
            }
        }
    }
}