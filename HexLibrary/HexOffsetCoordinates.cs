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

        public static HexAxial OffsetCoordinatesToAxial(int col, int row)
        {
            HexCube cube = OffsetCoordinatesToCube(col, row);
            HexAxial axial = HexCube.CubeToAxial(cube.X, cube.Y, cube.Z);

            return axial;
        }

        public static HexCube OffsetCoordinatesToCube(int col, int row)
        {
            int x = col - (row - (row & 1)) / 2;
            int z = row;
            int y = -x - z;
            HexCube cube = new HexCube(x, y, z);

            return cube;
        }

        public static HexOffsetCoordinates GetNeighbor(int col, int row, Direction direction)
        {
            HexCube cube = OffsetCoordinatesToCube(col, row);
            HexCube neighbor = HexCube.GetNeighbor(cube.X, cube.Y, cube.Z, direction);
            HexOffsetCoordinates offsetCoordinates = HexCube.CubeToOffsetCoordinates(neighbor.X, neighbor.Y, neighbor.Z);

            return offsetCoordinates;
        }

        // TODO: figure out how not to keep instantiating a new array
        public static HexOffsetCoordinates[] GetAllNeighbors(int col, int row)
        {
            HexCube cube = OffsetCoordinatesToCube(col, row);
            HexCube[] allNeighboringCubes = HexCube.GetAllNeighbors(cube.X, cube.Y, cube.Z);

            HexOffsetCoordinates[] neighbors = new HexOffsetCoordinates[HexCube.Directions.Length];
            for (int i = 0; i < HexCube.Directions.Length; ++i)
            {
                HexCube neighboringCube = allNeighboringCubes[i];
                neighbors[i] = HexCube.CubeToOffsetCoordinates(neighboringCube.X, neighboringCube.Y, neighboringCube.Z);
            }

            return neighbors;
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

            // TODO: fix bugs. Idea: use the returned coordinates to test against neighboring hexes and adjust to whichever is closest.

            return offsetCoordinates;
        }
    }
}