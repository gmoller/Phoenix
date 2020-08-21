using System;
using System.Collections.Generic;

namespace Utilities
{
    public class MapSolver : AStarSearch<Point, Cost>
    {
        private Func<Point, GetCostToMoveIntoResult> _getCostToMoveIntoFunc;
        private Point _gridSize;
        private Point _destination;
        private Dictionary<Point, Cost> _closedList;

        private Node? _solution;

        public override List<Point> Solution
        {
            get
            {
                if (!_solution.HasValue) return new List<Point>();

                var pos = _solution.Value.Position;
                var cost = _solution.Value.Cost;

                var result = new List<Point> { pos };
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

        public override void Solve(Func<Point, GetCostToMoveIntoResult> getCostToMoveIntoFunc, Point gridSize, Point start, Point destination, PriorityQueue<Node> openList, Dictionary<Point, Cost> closedList)
        {
            _getCostToMoveIntoFunc = getCostToMoveIntoFunc;
            _gridSize = gridSize;
            _closedList = closedList;
            _destination = destination;
            Solve(new Node(start, new Cost(-1, 0, GetDistance(start, _destination))), openList, closedList);
        }

        private int ToIndex(Point position) { return position.Y * _gridSize.X + position.X; }

        private Point ToPosition(int index) { return new Point(index % _gridSize.X, index / _gridSize.X); }

        protected override void AddNeighbors(Node node, PriorityQueue<Node> openList)
        {
            var parentIndex = ToIndex(node.Position);

            var neighbors = GetAllNeighbors(node.Position);
            foreach (var neighbor in neighbors)
            {
                var point = new Point(neighbor.X, neighbor.Y);
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

        protected override bool IsDestination(Point position)
        {
            var isSolved = position == _destination;
            if (isSolved) _solution = new Node(position, _closedList[position]);

            return isSolved;
        }
    }
}