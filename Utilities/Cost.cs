using System;
using System.Diagnostics;

namespace Utilities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct Cost : IComparable<Cost>
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