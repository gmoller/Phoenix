using System;

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
                _ => throw new ArgumentOutOfRangeException(nameof(OffsetCoordinatesType), OffsetCoordinatesType, $"OffsetCoordinatesType {OffsetCoordinatesType} is not supported."),
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
                _ => throw new NotSupportedException(
                    $"OffsetCoordinatesType [{OffsetCoordinatesType}] is not supported.")
            };

            var offsetCoordinates = new HexOffsetCoordinates(col, row);

            return offsetCoordinates;
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