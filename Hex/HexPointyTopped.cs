using System;
using Utilities;

namespace Hex
{
    public class HexPointyTopped : Hex
    {
        private static readonly HexCube[] Directions =
        {
            new HexCube( 0,  0,  0), // north
            new HexCube(+1,  0, -1), // northeast
            new HexCube(+1, -1,  0), // east
            new HexCube( 0, -1, +1), // southeast
            new HexCube( 0,  0,  0), // south
            new HexCube(-1,  0, +1), // southwest
            new HexCube(-1, +1,  0), // west
            new HexCube( 0, +1, -1), // northwest
        };

        public HexPointyTopped(OffsetCoordinatesType offsetCoordinatesType) : base(offsetCoordinatesType)
        {
        }


        protected override HexCube GetNeighboringCube(Direction direction)
        {
            var neighboringCube = Directions[(int)direction];

            return neighboringCube;
        }


        public override HexCube OffsetCoordinatesToCube(HexOffsetCoordinates hexOffsetCoordinates)
        {
            var col = hexOffsetCoordinates.Col;
            var row = hexOffsetCoordinates.Row;

            col -= OffsetCoordinatesType switch
            {
                OffsetCoordinatesType.Even => (row + (row & 1)) / 2,
                OffsetCoordinatesType.Odd => (row - (row & 1)) / 2,
                _ => throw new ArgumentOutOfRangeException(nameof(OffsetCoordinatesType), OffsetCoordinatesType, $"OffsetCoordinatesType {OffsetCoordinatesType} is not supported.")
            };
            var y = -col - row;
            var cube = new HexCube(col, y, row);

            return cube;
        }

        public override HexOffsetCoordinates CubeToOffsetCoordinates(HexCube hexCube)
        {
            var col = hexCube.X;
            var row = hexCube.Z;

            col += OffsetCoordinatesType switch
            {
                OffsetCoordinatesType.Even => (row + (row & 1)) / 2,
                OffsetCoordinatesType.Odd => (row - (row & 1)) / 2,
                _ => throw new ArgumentOutOfRangeException(nameof(OffsetCoordinatesType), OffsetCoordinatesType, $"OffsetCoordinatesType {OffsetCoordinatesType} is not supported.")
            };

            var offsetCoordinates = new HexOffsetCoordinates(col, row);

            return offsetCoordinates;
        }

        public override PointF FromOffsetCoordinatesToPixel(HexOffsetCoordinates offsetCoordinates)
        {
            var x = Constants.HexSize;
            var y = Constants.HexSize * (1.5f * offsetCoordinates.Row);

            x *= OffsetCoordinatesType switch
            {
                OffsetCoordinatesType.Even => Constants.SquareRootOf3 * (offsetCoordinates.Col - 0.5f * (offsetCoordinates.Row & 1)),
                OffsetCoordinatesType.Odd => Constants.SquareRootOf3 * (offsetCoordinates.Col + 0.5f * (offsetCoordinates.Row & 1)),
                _ => throw new ArgumentOutOfRangeException(nameof(OffsetCoordinatesType), OffsetCoordinatesType, $"OffsetCoordinatesType {OffsetCoordinatesType} is not supported.")
            };

            var pixel = new PointF((float)x, (float)y);

            return pixel;
        }

        public override HexAxial FromPixelToAxial(int x, int y)
        {
            var q = (Constants.OneThirdOfSquareRootOf3 * x - Constants.ONE_THIRD * y) / Constants.HexSize;
            var r = (Constants.TWO_THIRDS * y) / Constants.HexSize;
            var axial = RoundAxial((float)q, (float)r);

            return axial;
        }

        public override PointF FromAxialToPixel(HexAxial axial)
        {
            var x = Constants.HexSize * (Constants.SquareRootOf3 * axial.Q + Constants.HalfOfSquareRootOf3 * axial.R);
            var y = Constants.HexSize * (1.5f * axial.R);
            var pixel = new PointF((float)x, (float)y);

            return pixel;
        }

        protected override float GetDegreesForHexCorner(Direction direction)
        {
            var degrees = 0.0f;

            switch (direction)
            {
                case Direction.North:
                    degrees = 270;
                    break;
                case Direction.NorthEast:
                    degrees = 330;
                    break;
                case Direction.SouthEast:
                    degrees = 30;
                    break;
                case Direction.South:
                    degrees = 90;
                    break;
                case Direction.SouthWest:
                    degrees = 150;
                    break;
                case Direction.NorthWest:
                    degrees = 210;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return degrees;
        }
    }
}