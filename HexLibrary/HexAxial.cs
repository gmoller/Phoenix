using Microsoft.Xna.Framework;

namespace HexLibrary
{
    public struct HexAxial
    {
        public int Q { get; }
        public int R { get; }

        public HexAxial(int q, int r)
        {
            Q = q;
            R = r;
        }

        public static HexOffsetCoordinates AxialToOffsetCoordinates(int q, int r)
        {
            HexCube cube = AxialToCube(q, r);
            HexOffsetCoordinates offsetCoordinates = HexCube.CubeToOffsetCoordinates(cube.X, cube.Y, cube.Z);

            return offsetCoordinates;
        }

        public static HexCube AxialToCube(int q, int r)
        {
            int x = q;
            int z = r;
            int y = -x - z;
            HexCube cube = new HexCube(x, y, z);

            return cube;
        }

        public static HexAxial RoundAxial(float q, float r)
        {
            HexCube cube = HexCube.RoundCube(q, -q - r, r);
            HexAxial axial = HexCube.CubeToAxial(cube.X, cube.Y, cube.Z);

            return axial;
        }

        public static Vector2 AxialToPixel(int q, int r)
        {
            double x = Constants.HEX_SIZE_X * (Constants.SQUARE_ROOT_OF_3 * q + Constants.HALF_OF_SQUARE_ROOT_OF_3 * r);
            float y = Constants.HEX_SIZE_Y * (1.5f * r);
            Vector2 pixel = new Vector2((float)x, y);

            return pixel;
        }

        public static HexAxial AxialFromPixel(int x, int y)
        {
            double q = (Constants.ONE_THIRD_OF_SQUARE_ROOT_OF_3 * x - Constants.ONE_THIRD * y) / Constants.HEX_SIZE_X;
            float r = (Constants.TWO_THIRDS * y) / Constants.HEX_SIZE_Y;
            HexAxial axial = RoundAxial((float)q, r);

            return axial;
        }
    }
}