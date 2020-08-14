using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace HexLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct HexCube
    {
        public static readonly HexCube[] Directions =
        {
            new HexCube(+1, -1,  0), // east
            new HexCube( 0, -1, +1), // southeast
            new HexCube(-1,  0, +1), // southwest
            new HexCube(-1, +1,  0), // west
            new HexCube( 0, +1, -1), // northwest
            new HexCube(+1,  0, -1), // northeast
        };

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public HexCube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static HexOffsetCoordinates ToOffsetCoordinates(HexCube cube)
        {
            return ToOffsetCoordinates(cube.X, cube.Y, cube.Z);
        }

        public static HexOffsetCoordinates ToOffsetCoordinates(int x, int y, int z)
        {
            var col = x + (z - (z & 1)) / 2;
            var row = z;
            var offsetCoordinates = new HexOffsetCoordinates(col, row);

            return offsetCoordinates;
        }

        public static HexAxial ToAxial(HexCube cube)
        {
            return ToAxial(cube.X, cube.Y, cube.Z);
        }

        public static HexAxial ToAxial(int x, int y, int z)
        {
            var q = x;
            var r = z;
            var axial = new HexAxial(q, r);

            return axial;
        }

        public static HexCube GetNeighbor(HexCube cube, Direction direction)
        {
            return GetNeighbor(cube.X, cube.Y, cube.Z, direction);
        }

        public static HexCube GetNeighbor(int x, int y, int z, Direction direction)
        {
            var offset = Directions[(int)direction];
            var neighbor = new HexCube(x + offset.X, y + offset.Y, z + offset.Z);

            return neighbor;
        }

        public static HexCube[] GetAllNeighbors(HexCube cube)
        {
            return GetAllNeighbors(cube.X, cube.Y, cube.Z);
        }

        public static HexCube[] GetAllNeighbors(int x, int y, int z)
        {
            var neighbors = new HexCube[Directions.Length];
            for (var i = 0; i < Directions.Length; ++i)
            {
                neighbors[i] = GetNeighbor(x, y, z, (Direction)i);
            }

            return neighbors;
        }

        public static HexCube Add(HexCube a, HexCube b)
        {
            return new HexCube(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static HexCube Scale(HexCube a, int k)
        {
            return new HexCube(a.X * k, a.Y * k, a.Z * k);
        }

        public static HexCube Round(float x, float y, float z)
        {
            var rx = (int)Math.Round(x);
            var ry = (int)Math.Round(y);
            var rz = (int)Math.Round(z);

            var zero = rx + ry + rz;

            var xDiff = Math.Abs(rx - x);
            var yDiff = Math.Abs(ry - y);
            var zDiff = Math.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff)
            {
                rx = -ry - rz;
            }
            else if (yDiff > zDiff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }
            var cube = new HexCube(rx, ry, rz);

            return cube;
        }

        public static Vector2 ToPixel(HexCube cube)
        {
            return ToPixel(cube.X, cube.Y, cube.Z);
        }

        public static Vector2 ToPixel(int x, int y, int z)
        {
            var axial = ToAxial(x, y, z);
            var point = HexAxial.ToPixel(axial.Q, axial.R);

            return point;
        }

        public static HexCube FromPixel(int x, int y)
        {
            var q = (Constants.OneThirdOfSquareRootOf3 * x - Constants.ONE_THIRD * y) / Constants.HexSize;
            var r = (Constants.TWO_THIRDS * y) / Constants.HexSize;
            var axial = HexAxial.Round((float)q, (float)r);
            var cube = HexAxial.ToCube(axial);

            return cube;
        }

        public static int GetDistance(HexCube a, HexCube b)
        {
            return GetDistance(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static int GetDistance(int fromCubeX, int fromCubeY, int fromCubeZ, int toCubeX, int toCubeY, int toCubeZ)
        {
            var diffX = Math.Abs(fromCubeX - toCubeX);
            var diffY = Math.Abs(fromCubeY - toCubeY);
            var diffZ = Math.Abs(fromCubeZ - toCubeZ);
            var distance1 = (diffX + diffY + diffZ) / 2;
            var distance2 = Math.Max(Math.Max(diffX, diffY), diffZ);

            if (distance1 != distance2) throw new Exception("distance1not equal to distance2!");

            return distance1;
        }

        public static (float x, float y, float z) Lerp(HexCube a, HexCube b, float t)
        {
            return Lerp(a.X, a.Y, a.Z, b.X, b.Y, b.Z, t);
        }

        public static (float x, float y, float z) Lerp(int fromCubeX, int fromCubeY, int fromCubeZ, int toCubeX, int toCubeY, int toCubeZ,  float t)
        {
            var x = Lerp(fromCubeX, toCubeX, t);
            var y = Lerp(fromCubeY, toCubeY, t);
            var z = Lerp(fromCubeZ, toCubeZ, t);

            return (x, y, z);
        }

        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static List<HexCube> GetLine(HexCube a, HexCube b)
        {
            return GetLine(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public static List<HexCube> GetLine(int fromCubeX, int fromCubeY, int fromCubeZ, int toCubeX, int toCubeY, int toCubeZ)
        {
            var distance = GetDistance(fromCubeX, fromCubeY, fromCubeZ, toCubeX, toCubeY, toCubeZ);

            var results = new List<HexCube>();
            for (var i = 0; i <= distance; ++i)
            {
                var t = 1.0f / distance * i;
                var lerp = Lerp(fromCubeX, fromCubeY, fromCubeZ, toCubeX, toCubeY, toCubeZ, t);
                var hex = Round(lerp.x, lerp.y, lerp.z);
                results.Add(hex);
            }

            return results;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{X={X},Y={Y},Z={Z}}}";
    }
}