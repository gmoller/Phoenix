using System;
using Microsoft.Xna.Framework;

namespace HexLibrary
{
    public struct HexCube
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

        public static HexAxial CubeToAxial(int x, int y, int z)
        {
            int q = x;
            int r = z;
            HexAxial axial = new HexAxial(q, r); ;

            return axial;
        }

        public static HexOffsetCoordinates CubeToOffsetCoordinates(int x, int y, int z)
        {
            int col = x + (z - (z & 1)) / 2;
            int row = z;
            HexOffsetCoordinates offsetCoordinates = new HexOffsetCoordinates(col, row);

            return offsetCoordinates;
        }

        public static HexCube RoundCube(float x, float y, float z)
        {
            int rx = (int)Math.Round(x);
            int ry = (int)Math.Round(y);
            int rz = (int)Math.Round(z);

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
            double q = (Constants.ONE_THIRD_OF_SQUARE_ROOT_OF_3 * x - Constants.ONE_THIRD * y) / Constants.HEX_SIZE_X;
            float r = (Constants.TWO_THIRDS * y) / Constants.HEX_SIZE_Y;
            HexAxial axial = HexAxial.RoundAxial((float)q, r);
            HexCube cube = HexAxial.AxialToCube(axial.Q, axial.R);

            return cube;
        }
    }
}