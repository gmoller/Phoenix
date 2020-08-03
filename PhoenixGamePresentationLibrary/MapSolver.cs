using System.Collections.Generic;
using HexLibrary;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class MapSolver : AStarSearch<Point, Cost>
    {
        private Unit _unit;
        private Point _gridSize;
        private Point _destination;
        private Dictionary<Point, Cost> _closedList;

        public Node? Solution { get; private set; }

        public void Graph(Unit unit, Point gridSize, Point start, Point destination, PriorityQueue<Node> openList, Dictionary<Point, Cost> closedList)
        {
            _unit = unit;
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
                var canMoveInto = _unit.CostToMoveInto(point);
                if (!canMoveInto.canMoveInto) continue;

                var distanceCost = node.Cost.DistanceTraveled + canMoveInto.costToMoveInto;
                var cost = new Cost(parentIndex, (int)distanceCost, (int)distanceCost + GetDistance(point, _destination));
                openList.Enqueue(new Node(point, cost));
            }
        }

        protected override bool IsDestination(Point position)
        {
            bool isSolved = position == _destination;
            if (isSolved) Solution = new Node(position, _closedList[position]);

            return isSolved;
        }

        private static int GetDistance(Point source, Point destination)
        {
            var distance = HexOffsetCoordinates.GetDistance(source.X, source.Y, destination.X, destination.Y);

            return distance;
        }
    }
}