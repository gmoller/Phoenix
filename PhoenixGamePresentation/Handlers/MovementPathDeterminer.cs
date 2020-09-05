using System.Collections.Generic;
using Hex;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class MovementPathDeterminer
    {
        internal static List<PointI> DetermineMovementPath(StackView stackView, PointI from, PointI to, CellGrid cellGrid)
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
                return stackView.GetCostToMoveInto(point);
            }
        }

        private static int GetDistance(PointI source, PointI destination)
        {
            var distance = HexOffsetCoordinates.GetDistance(source.X, source.Y, destination.X, destination.Y);

            return distance;
        }
         
        private static PointI[] GetAllNeighbors(PointI point)
        {
            var allNeighborsHexOffsetCoordinates =  HexOffsetCoordinates.GetAllNeighbors(point.X, point.Y);

            var allNeighbors  = new PointI[allNeighborsHexOffsetCoordinates.Length];
            for (int i = 0; i < allNeighborsHexOffsetCoordinates.Length; ++i)
            {
                var allNeighborsHexOffsetCoordinate = allNeighborsHexOffsetCoordinates[i];
                allNeighbors[i] = new PointI(allNeighborsHexOffsetCoordinate.Col, allNeighborsHexOffsetCoordinate.Row);
            }

            return allNeighbors;
        }
    }
}