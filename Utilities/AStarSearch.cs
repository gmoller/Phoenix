using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Utilities
{
    public abstract class AStarSearch<TKey, TValue> where TValue : IComparable<TValue>
    {
        #region State
        public Func<Point, Point[]> GetAllNeighbors { get; set; }
        public Func<Point, Point, int> GetDistance { get; set; }

        public abstract List<Point> Solution { get; }
        #endregion

        public abstract void Solve(Func<Point, GetCostToMoveIntoResult> getCostToMoveIntoFunc, Point gridSize, Point start,  Point destination, PriorityQueue<Node> openList, Dictionary<Point, Cost> closedList);

        protected void Solve(Node start, PriorityQueue<Node> openList, Dictionary<TKey, TValue> closedList)
        {
            openList.Enqueue(start);
            while (openList.Count > 0)
            {
                var node = openList.RemoveRoot();

                if (closedList.ContainsKey(node.Position))
                {
                    continue;
                }

                closedList.Add(node.Position, node.Cost);

                if (IsDestination(node.Position))
                {
                    return;
                }

                AddNeighbors(node, openList);
            }
        }

        protected abstract void AddNeighbors(Node node, PriorityQueue<Node> openList);

        protected abstract bool IsDestination(TKey position);

        [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
        public struct Node : IComparable<Node>
        {
            public TKey Position { get; }
            public TValue Cost { get; }

            public Node(TKey position, TValue cost)
            {
                Position = position;
                Cost = cost;
            }

            public int CompareTo(Node other) { return Cost.CompareTo(other.Cost); }

            public override string ToString()
            {
                return DebuggerDisplay;
            }

            private string DebuggerDisplay => $"{{Position={Position},Cost={Cost}}}";
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Cost : IComparable<Cost>
    {
        public int ParentIndex { get; }
        public int DistanceTraveled { get; } /*g(x)*/
        private int TotalCost { get; } /*f(x)*/

        public Cost(int parentIndex, int distanceTraveled, int totalCost)
        {
            ParentIndex = parentIndex;
            DistanceTraveled = distanceTraveled;
            TotalCost = totalCost;
        }

        public int CompareTo(Cost other) { return TotalCost.CompareTo(other.TotalCost); }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{ParentIndex={ParentIndex},DistanceTraveled={DistanceTraveled},TotalCost={TotalCost}}}";
    }
}