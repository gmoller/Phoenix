using System;
using System.Collections.Generic;

namespace Utilities
{
    public class MapSolver : AStarSearch<PointI, Cost>
    {
        private Func<PointI, GetCostToMoveIntoResult> _getCostToMoveIntoFunc;
        private PointI _gridSize;
        private PointI _destination;
        private Dictionary<PointI, Cost> _closedList;

        private Node? _solution;

        public override List<PointI> Solution
        {
            get
            {
                if (!_solution.HasValue) return new List<PointI>();

                var pos = _solution.Value.Position;
                var cost = _solution.Value.Cost;

                var result = new List<PointI> { pos };
                do
                {
                    pos = ToPosition(cost.ParentIndex);
                    cost = _closedList[pos];
                    result.Add(pos);
                } while (cost.ParentIndex >= 0);

                result.RemoveAt(result.Count - 1);
                result.Reverse();

                return result;
            }
        }

        public override void Solve(Func<PointI, GetCostToMoveIntoResult> getCostToMoveIntoFunc, PointI gridSize, PointI start, PointI destination, PriorityQueue<Node> openList, Dictionary<PointI, Cost> closedList)
        {
            _getCostToMoveIntoFunc = getCostToMoveIntoFunc;
            _gridSize = gridSize;
            _closedList = closedList;
            _destination = destination;
            Solve(new Node(start, new Cost(-1, 0, GetDistance(start, _destination))), openList, closedList);
        }

        private int ToIndex(PointI position) { return position.Y * _gridSize.X + position.X; }

        private PointI ToPosition(int index) { return new PointI(index % _gridSize.X, index / _gridSize.X); }

        protected override void AddNeighbors(Node node, PriorityQueue<Node> openList)
        {
            var parentIndex = ToIndex(node.Position);

            var neighbors = GetAllNeighbors(node.Position);
            foreach (var neighbor in neighbors)
            {
                var point = new PointI(neighbor.X, neighbor.Y);
                var costToMoveIntoResult = _getCostToMoveIntoFunc(point);
                if (!costToMoveIntoResult.CanMoveInto)
                {
                    continue;
                }

                var distanceCost = node.Cost.DistanceTraveled + costToMoveIntoResult.CostToMoveInto;
                var cost = new Cost(parentIndex, (int)distanceCost, (int)distanceCost + GetDistance(point, _destination));
                openList.Enqueue(new Node(point, cost));
            }
        }

        protected override bool IsDestination(PointI position)
        {
            var isSolved = position == _destination;
            if (isSolved) _solution = new Node(position, _closedList[position]);

            return isSolved;
        }
    }
}