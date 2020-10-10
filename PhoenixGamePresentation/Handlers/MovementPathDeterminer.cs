using System.Collections.Generic;
using Hex;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class MovementPathDeterminer
    {
        private static readonly HexLibrary HexLibrary = new HexLibrary(HexType.PointyTopped, OffsetCoordinatesType.Odd);

        internal static List<PointI> DetermineMovementPath(Stack stack, PointI from, PointI to, CellGrid cellGrid)
        {
            if (from.Equals(to)) return new List<PointI>();

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
            var distance = HexLibrary.GetDistance(new HexOffsetCoordinates(source), new HexOffsetCoordinates(destination));

            return distance;
        }

        private static PointI[] GetAllNeighbors(PointI point)
        {
            var neighbors = HexLibrary.GetAllNeighbors(new HexOffsetCoordinates(point));

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