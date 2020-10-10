using System.Diagnostics;

namespace Hex
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct HexAxial
    {
        public int Q { get; }
        public int R { get; }

        public HexAxial(int q, int r)
        {
            Q = q;
            R = r;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Q={Q},R={R}}}";
    }
}