using System.Collections.Generic;
using PhoenixGameLibrary;
using Utilities;
using Zen.Hexagons;

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

            var allNeighbors  = new PointI[6];
            for (int i = 0; i < 6; ++i)
            {
                var allNeighborsHexOffsetCoordinate = neighbors[i];
                allNeighbors[i] = new PointI(allNeighborsHexOffsetCoordinate.Col, allNeighborsHexOffsetCoordinate.Row);
            }

            return allNeighbors;
        }
    }
}