using System;
using Microsoft.Xna.Framework;

namespace HexLibrary
{
    public struct HexCube
    {
        public static readonly HexCube[] Directions =
        {
            new HexCube(+1, -1,  0), // east
            new HexCube( 0, -1, +1), // southeast
            new HexCube(-1,  0, +1), // southwest
            new HexCube(-1, +1,  0), // west
            new HexCube( 0, +1, -1), // northwest
            new HexCube(+1,  0, -1), // northeast,
            //new HexCube(+2, -1, -1), // EastOfNorthEast
            //new HexCube(+2, -2,  0), // EastOfEast
            //new HexCube(+1, -2, +1), // EastOfSouthEast
            //new HexCube( 0, -2, +2), // SouthEastOfSouthEast
            //new HexCube(-1, -1, +2), // SouthEastOfSouthWest,
            //new HexCube(-2,  0, +2), // SouthWestOfSouthWest,
            //new HexCube(-2, +1, +1), // SouthWestOfWest,
            //new HexCube(-2, +2,  0), // WestofWest,
            //new HexCube(-1, +2, -1), // NorthWestofWest,
            //new HexCube( 0, +2, -2), // NorthWestofNorthWest,
            //new HexCube(+1, +1, -2), // NorthEastofNorthWest,
            //new HexCube(+2,  0, -2), // NorthEastOfNorthEast,
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

        public static HexOffsetCoordinates CubeToOffsetCoordinates(int x, int y, int z)
        {
            int col = x + (z - (z & 1)) / 2;
            int row = z;
            HexOffsetCoordinates offsetCoordinates = new HexOffsetCoordinates(col, row);

            return offsetCoordinates;
        }

        public static HexAxial CubeToAxial(int x, int y, int z)
        {
            int q = x;
            int r = z;
            HexAxial axial = new HexAxial(q, r); ;

            return axial;
        }

        public static HexCube GetNeighbor(int x, int y, int z, Direction direction)
        {
            HexCube offset = Directions[(int)direction];
            HexCube neighbor = new HexCube(x + offset.X, y + offset.Y, z + offset.Z);

            return neighbor;
        }

        // TODO: figure out how not to keep instantiating a new array
        public static HexCube[] GetAllNeighbors(int x, int y, int z)
        {
            HexCube[] neighbors = new HexCube[Directions.Length];
            for (int i = 0; i < Directions.Length; ++i)
            {
                neighbors[i] = GetNeighbor(x, y, z, (Direction)i);
            }

            return neighbors;
        }

        public static HexCube AddHex(HexCube a, HexCube b)
        {
            return new HexCube(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static HexCube ScaleHex(HexCube a, int k)
        {
            return new HexCube(a.X * k, a.Y * k, a.Z * k);
        }

        public static HexCube RoundCube(float x, float y, float z)
        {
            int rx = (int)Math.Round(x);
            int ry = (int)Math.Round(y);
            int rz = (int)Math.Round(z);

            int zero = rx + ry + rz;

            float xDiff = Math.Abs(rx - x);
            float yDiff = Math.Abs(ry - y);
            float zDiff = Math.Abs(rz - z);

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
            HexCube cube = new HexCube(rx, ry, rz);

            return cube;
        }

        public static Vector2 CubeToPixel(int x, int y, int z)
        {
            HexAxial axial = CubeToAxial(x, y, z);
            Vector2 point = HexAxial.AxialToPixel(axial.Q, axial.R);

            return point;
        }

        public static HexCube CubeFromPixel(int x, int y)
        {
            double q = (Constants.ONE_THIRD_OF_SQUARE_ROOT_OF_3 * x - Constants.ONE_THIRD * y) / Constants.HEX_SIZE;
            float r = (Constants.TWO_THIRDS * y) / Constants.HEX_SIZE;
            HexAxial axial = HexAxial.RoundAxial((float)q, r);
            HexCube cube = HexAxial.AxialToCube(axial.Q, axial.R);

            return cube;
        }
    }
}