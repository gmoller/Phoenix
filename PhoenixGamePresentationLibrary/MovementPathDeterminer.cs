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
            mapSolver.Graph(unit, Globals.Instance.World.OverlandMap.CellGrid, from, to, openList, closedList);
            if (mapSolver.Solution.HasValue)
            {
                var pos = mapSolver.Solution.Value.Position;
                var cost = mapSolver.Solution.Value.Cost;

                var result = new List<Point> { pos };
                do
                {
                    pos = mapSolver.ToPosition(cost.ParentIndex);
                    cost = closedList[pos];
                    result.Add(pos);
                } while (cost.ParentIndex >= 0);

                result.RemoveAt(result.Count - 1);
                result.Reverse();

                return result;
            }

            return new List<Point>();
        }
    }
}