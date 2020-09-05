using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utilities;

namespace Hex
{
    // odd-R
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

        public static HexAxial ToAxial(HexOffsetCoordinates offsetCoordinates)
        {
            return ToAxial(offsetCoordinates.Col, offsetCoordinates.Row);
        }

        public static HexAxial ToAxial(int col, int row)
        {
            var cube = ToCube(col, row);
            var axial = HexCube.ToAxial(cube);

            return axial;
        }

        public static HexCube ToCube(HexOffsetCoordinates offsetCoordinates)
        {
            return ToCube(offsetCoordinates.Col, offsetCoordinates.Row);
        }

        public static HexCube ToCube(int col, int row)
        {
            var x = col - (row - (row & 1)) / 2;
            var z = row;
            var y = -x - z;
            var cube = new HexCube(x, y, z);

            return cube;
        }

        public static HexOffsetCoordinates GetNeighbor(HexOffsetCoordinates offsetCoordinates, Direction direction)
        {
            return GetNeighbor(offsetCoordinates.Col, offsetCoordinates.Row, direction);
        }

        public static HexOffsetCoordinates GetNeighbor(int col, int row, Direction direction)
        {
            var cube = ToCube(col, row);
            var neighbor = HexCube.GetNeighbor(cube, direction);
            var offsetCoordinates = HexCube.ToOffsetCoordinates(neighbor);

            return offsetCoordinates;
        }

        public static HexOffsetCoordinates[] GetAllNeighbors(HexOffsetCoordinates offsetCoordinates)
        {
            return GetAllNeighbors(offsetCoordinates.Col, offsetCoordinates.Row);
        }

        public static HexOffsetCoordinates[] GetAllNeighbors(int col, int row)
        {
            var cube = ToCube(col, row);
            var allNeighboringCubes = HexCube.GetAllNeighbors(cube.X, cube.Y, cube.Z);

            var neighbors = new HexOffsetCoordinates[HexCube.Directions.Length];
            for (var i = 0; i < HexCube.Directions.Length; ++i)
            {
                var neighboringCube = allNeighboringCubes[i];
                neighbors[i] = HexCube.ToOffsetCoordinates(neighboringCube);
            }

            return neighbors;
        }

        public static HexOffsetCoordinates[] GetSpiralRing(HexOffsetCoordinates offsetCoordinates, int radius)
        {
            return GetSpiralRing(offsetCoordinates.Col, offsetCoordinates.Row, radius);
        }

        public static HexOffsetCoordinates[] GetSpiralRing(int col, int row, int radius)
        {
            var ring = new List<HexOffsetCoordinates> { new HexOffsetCoordinates(col, row) };

            for (var k = 1; k <= radius; ++k)
            {
                var o = GetSingleRing(col, row, k);
                ring.AddRange(o.ToList());
            }

            return ring.ToArray();
        }

        public static HexOffsetCoordinates[] GetSingleRing(HexOffsetCoordinates offsetCoordinates, int radius)
        {
            return GetSingleRing(offsetCoordinates.Col, offsetCoordinates.Row, radius);
        }

        public static HexOffsetCoordinates[] GetSingleRing(int col, int row, int radius)
        {
            var ring = new List<HexOffsetCoordinates>();

            var cube = ToCube(col, row);
            var scaledCube = HexCube.Scale(HexCube.Directions[4], radius);
            cube = HexCube.Add(cube, scaledCube);

            for (var i = 0; i < 6; ++i)
            {
                for (var j = 0; j < radius; ++j)
                {
                    var offset = HexCube.ToOffsetCoordinates(cube);
                    ring.Add(offset);
                    cube = HexCube.GetNeighbor(cube, (Direction)i);
                }
            }

            return ring.ToArray();
        }

        //public static HexOffsetCoordinates Round(float col, float row)
        //{
        //    var cube = HexCube.Round(q, -q - r, r);
        //    var offsetCoordinates = HexCube.ToOffsetCoordinates(cube);
        //
        //    return offsetCoordinates;
        //}

        public static PointF ToPixel(HexOffsetCoordinates offsetCoordinates)
        {
            return ToPixel(offsetCoordinates.Col, offsetCoordinates.Row);
        }

        public static PointF ToPixel(int col, int row)
        {
            var x = Constants.HexSize * (Constants.SquareRootOf3 * (col + 0.5f * (row & 1)));
            var y = Constants.HexSize * (1.5f * row);
            var pixel = new PointF((float)x, (float)y);

            return pixel;
        }

        public static HexOffsetCoordinates FromPixel(HexOffsetCoordinates offsetCoordinates)
        {
            return FromPixel(offsetCoordinates.Col, offsetCoordinates.Row);
        }

        public static HexOffsetCoordinates FromPixel(int x, int y)
        {
            return FromPixel(x, (double)y);
        }

        public static HexOffsetCoordinates FromPixel(PointF p)
        {
            return FromPixel(p.X, p.Y);
        }

        public static HexOffsetCoordinates FromPixel(double x, double y)
        {
            var cube = HexCube.FromPixel(x, y);
            var offsetCoordinates = HexCube.ToOffsetCoordinates(cube);

            return offsetCoordinates;
        }

        public static int GetDistance(HexOffsetCoordinates from, HexOffsetCoordinates to)
        {
            return GetDistance(from.Col, from.Row, to.Col, to.Row);
        }

        public static int GetDistance(int fromCol, int fromRow, int toCol, int toRow)
        {
            var fromCube = ToCube(fromCol, fromRow);
            var toCube = ToCube(toCol, toRow);

            var distance = HexCube.GetDistance(fromCube, toCube);

            return distance;
        }

        public static List<HexOffsetCoordinates> GetLine(HexOffsetCoordinates from, HexOffsetCoordinates to)
        {
            return GetLine(from.Col, from.Row, to.Col, to.Row);
        }

        public static List<HexOffsetCoordinates> GetLine(int fromCol, int fromRow, int toCol, int toRow)
        {
            var fromCube = ToCube(fromCol, fromRow);
            var toCube = ToCube(toCol, toRow);

            var hexes = HexCube.GetLine(fromCube.X, fromCube.Y, fromCube.Z, toCube.X, toCube.Y, toCube.Z);

            var result = new List<HexOffsetCoordinates>();
            foreach (var hex in hexes)
            {
                var o = HexCube.ToOffsetCoordinates(hex);
                result.Add(o);
            }

            return result;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Col={Col},Row={Row}}}";
    }
}