using System;
using System.Collections.Generic;
using HexLibrary;

namespace Utilities
{
    public class MapSolver : AStarSearch<Point, Cost>
    {
        private Func<Point, CostToMoveIntoResult> _getCostToMoveIntoFunc;
        private Point _gridSize;
        private Point _destination;
        private Dictionary<Point, Cost> _closedList;

        private Node? _solution;

        public List<Point> Solution
        {
            get
            {
                if (_solution.HasValue)
                {
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
                else
                {
                    return new List<Point>();
                }
            }
        }

        public void Graph(Func<Point, CostToMoveIntoResult> getCostToMoveIntoFunc, Point gridSize, Point start, Point destination, PriorityQueue<Node> openList, Dictionary<Point, Cost> closedList)
        {
            _getCostToMoveIntoFunc = getCostToMoveIntoFunc;
            _gridSize = gridSize;
            _closedList = closedList;
            _destination = destination;
            Graph(new Node(start, new Cost(-1, 0, GetDistance(start, _destination))), openList, closedList);
        }

        public int ToIndex(Point position) { return position.Y * _gridSize.X + position.X; }

        public Point ToPosition(int index) { return new Point(index % _gridSize.X, index / _gridSize.X); }

        protected override void AddNeighbors(Node node, PriorityQueue<Node> openList)
        {
            var parentIndex = ToIndex(node.Position);

            var neighbors = HexOffsetCoordinates.GetAllNeighbors(node.Position.X, node.Position.Y);
            foreach (var neighbor in neighbors)
            {
                // is within bounds of map?
                if (neighbor.Col < 0 || neighbor.Col >= _gridSize.X || neighbor.Row < 0 ||
                    neighbor.Row >= _gridSize.Y) continue;

                var point = new Point(neighbor.Col, neighbor.Row);
                var costToMoveIntoResult = _getCostToMoveIntoFunc.Invoke(point);
                if (!costToMoveIntoResult.CanMoveInto) continue;

                var distanceCost = node.Cost.DistanceTraveled + costToMoveIntoResult.CostToMoveInto;
                var cost = new Cost(parentIndex, (int)distanceCost, (int)distanceCost + GetDistance(point, _destination));
                openList.Enqueue(new Node(point, cost));
            }
        }

        protected override bool IsDestination(Point position)
        {
            bool isSolved = position == _destination;
            if (isSolved) _solution = new Node(position, _closedList[position]);

            return isSolved;
        }

        private static int GetDistance(Point source, Point destination)
        {
            var distance = HexOffsetCoordinates.GetDistance(source.X, source.Y, destination.X, destination.Y);

            return distance;
        }
    }
}