using System.Collections.Generic;
using HexLibrary;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal static class MovementPathDeterminer
    {
        internal static List<Point> DetermineMovementPath(StackView stackView, Point from, Point to, World world)
        {
            if (from.Equals(to)) return new List<Point>();

            AStarSearch<Point, Cost> mapSolver = new MapSolver();
            mapSolver.GetAllNeighbors = GetAllNeighbors;
            mapSolver.GetDistance = GetDistance;
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var cellGrid = world.OverlandMap.CellGrid;
            mapSolver.Solve(GetCostToMoveIntoFunc, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), from, to, openList, closedList);

            var path = mapSolver.Solution;

            return path;

            GetCostToMoveIntoResult GetCostToMoveIntoFunc(Point point)
            {
                return stackView.GetCostToMoveInto(point);
            }
        }

        private static int GetDistance(Point source, Point destination)
        {
            var distance = HexOffsetCoordinates.GetDistance(source.X, source.Y, destination.X, destination.Y);

            return distance;
        }
         
        private static Point[] GetAllNeighbors(Point point)
        {
            var allNeighborsHexOffsetCoordinates =  HexOffsetCoordinates.GetAllNeighbors(point.X, point.Y);

            var allNeighbors  = new Point[allNeighborsHexOffsetCoordinates.Length];
            for (int i = 0; i < allNeighborsHexOffsetCoordinates.Length; ++i)
            {
                var allNeighborsHexOffsetCoordinate = allNeighborsHexOffsetCoordinates[i];
                allNeighbors[i] = new Point(allNeighborsHexOffsetCoordinate.Col, allNeighborsHexOffsetCoordinate.Row);
            }

            return allNeighbors;
        }
    }
}