using System.Collections.Generic;
using PhoenixGameLibrary;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class MovementPathDeterminer
    {
        private static HexLibrary _hexLibrary;

        internal static List<PointI> DetermineMovementPath(HexLibrary hexLibrary, Stack stack, PointI from, PointI to, CellGrid cellGrid)
        {
            if (from.Equals(to)) return new List<PointI>();

            _hexLibrary = hexLibrary;
            AStarSearch<PointI, Cost> mapSolver = new MapSolver();
            mapSolver.GetAllNeighbors = GetAllNeighbors;
            mapSolver.GetDistance = GetDistance;
            var openList = new PriorityQueue<AStarSearch<PointI, Cost>.Node>();
            var closedList = new Dictionary<PointI, Cost>();

            mapSolver.Solve(GetCostToMoveIntoFunc, new PointI(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), from, to, openList, closedList);

            var path = mapSolver.Solution;

            return path;

            GetCostToMoveIntoResult GetCostToMoveIntoFunc(PointI point)
            {
                return stack.GetCostToMoveInto(point);
            }
        }

        private static int GetDistance(PointI source, PointI destination)
        {
            var distance = _hexLibrary.GetDistance(new HexOffsetCoordinates(source.X, source.Y), new HexOffsetCoordinates(destination.X, destination.Y));

            return distance;
        }

        private static PointI[] GetAllNeighbors(PointI point)
        {
            var neighbors = _hexLibrary.GetAllNeighbors(new HexOffsetCoordinates(point.X, point.Y));

            var allNeighbors = new PointI[6];
            allNeighbors[0] = new PointI(neighbors.HexOffsetCoordinates0.Col, neighbors.HexOffsetCoordinates0.Row);
            allNeighbors[1] = new PointI(neighbors.HexOffsetCoordinates1.Col, neighbors.HexOffsetCoordinates1.Row);
            allNeighbors[2] = new PointI(neighbors.HexOffsetCoordinates2.Col, neighbors.HexOffsetCoordinates2.Row);
            allNeighbors[3] = new PointI(neighbors.HexOffsetCoordinates3.Col, neighbors.HexOffsetCoordinates3.Row);
            allNeighbors[4] = new PointI(neighbors.HexOffsetCoordinates4.Col, neighbors.HexOffsetCoordinates4.Row);
            allNeighbors[5] = new PointI(neighbors.HexOffsetCoordinates5.Col, neighbors.HexOffsetCoordinates5.Row);

            return allNeighbors;
        }
    }
}