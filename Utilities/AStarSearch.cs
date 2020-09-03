using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Utilities
{
    public abstract class AStarSearch<TKey, TValue> where TValue : IComparable<TValue>
    {
        #region State
        public Func<PointI, PointI[]> GetAllNeighbors { get; set; }
        public Func<PointI, PointI, int> GetDistance { get; set; }

        public abstract List<PointI> Solution { get; }
        #endregion

        public abstract void Solve(Func<PointI, GetCostToMoveIntoResult> getCostToMoveIntoFunc, PointI gridSize, PointI start,  PointI destination, PriorityQueue<Node> openList, Dictionary<PointI, Cost> closedList);

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
        public readonly struct Node : IComparable<Node>
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
}