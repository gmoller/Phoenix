using System.Diagnostics;

namespace Hex
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct HexCube
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public HexCube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static HexCube operator + (HexCube a, HexCube b)
        {
            var hexCube = new HexCube(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

            return hexCube;
        }

        public static HexCube operator * (HexCube a, int k)
        {
            var hexCube = a.Scale(k);

            return hexCube;
        }

        public HexCube Scale(int k)
        {
            var scaledCube = new HexCube(X * k, Y * k, Z * k);

            return scaledCube;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{X={X},Y={Y},Z={Z}}}";
    }
}