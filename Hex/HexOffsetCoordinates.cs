using System.Diagnostics;
using Utilities;

namespace Hex
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct HexOffsetCoordinates
    {
        public int Col { get; }
        public int Row { get; }

        public HexOffsetCoordinates(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public HexOffsetCoordinates(PointI p)
        {
            Col = p.X;
            Row = p.Y;
        }

        public PointI ToPointI() => new PointI(Col, Row);

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Col={Col},Row={Row}}}";
    }
}