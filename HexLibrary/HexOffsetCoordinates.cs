using Microsoft.Xna.Framework;

namespace HexLibrary
{
    // odd-R
    public struct HexOffsetCoordinates
    {
        public int Col { get; }
        public int Row { get; }

        public HexOffsetCoordinates(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public static HexCube OffsetCoordinatesToCube(int col, int row)
        {
            int x = col - (row - (row & 1)) / 2;
            int z = row;
            int y = -x - z;
            HexCube cube = new HexCube(x, y, z);

            return cube;
        }

        public static HexAxial OffsetCoordinatesToAxial(int col, int row)
        {
            HexCube cube = OffsetCoordinatesToCube(col, row);
            HexAxial axial = HexCube.CubeToAxial(cube.X, cube.Y, cube.Z);

            return axial;
        }

        //public static HexOffsetCoordinates RoundOffsetCoordinates(float col, float row)
        //{
        //    HexCube cube = HexCube.RoundCube(q, -q - r, r);
        //    HexOffsetCoordinates offsetCoordinates = HexCube.CubeToOffsetCoordinates(cube.X, cube.Y, cube.Z);
        //
        //    return offsetCoordinates;
        //}

        public static Vector2 OffsetCoordinatesToPixel(int col, int row)
        {
            double x = Constants.HEX_SIZE_X * (Constants.SQUARE_ROOT_OF_3 * (col + 0.5f * (row & 1)));
            float y = Constants.HEX_SIZE_Y * (1.5f * row);
            Vector2 pixel = new Vector2((float)x, y);

            return pixel;
        }

        public static HexOffsetCoordinates OffsetCoordinatesFromPixel(int x, int y)
        {
            HexCube cube = HexCube.CubeFromPixel(x, y);
            HexOffsetCoordinates offsetCoordinates = HexCube.CubeToOffsetCoordinates(cube.X, cube.Y, cube.Z);

            return offsetCoordinates;
        }
    }
}