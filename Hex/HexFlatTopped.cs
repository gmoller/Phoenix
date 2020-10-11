﻿using System;

namespace Hex
{
    public class HexFlatTopped : Hex
    {
        private static readonly HexCube[] Directions =
        {
            new HexCube( 0, +1, -1), // north
            new HexCube(+1,  0, -1), // northeast
            new HexCube( 0,  0,  0), // east
            new HexCube(+1, -1,  0), // southeast
            new HexCube( 0, -1, +1), // south
            new HexCube(-1,  0, +1), // southwest
            new HexCube( 0,  0,  0), // west
            new HexCube(-1, +1,  0), // northwest
        };

        public HexFlatTopped(OffsetCoordinatesType offsetCoordinatesType) : base(offsetCoordinatesType)
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

            row -= OffsetCoordinatesType switch
            {
                OffsetCoordinatesType.Even => (col + (col & 1)) / 2,
                OffsetCoordinatesType.Odd => (col - (col & 1)) / 2,
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

            row += OffsetCoordinatesType switch
            {
                OffsetCoordinatesType.Even => (col + (col & 1)) / 2,
                OffsetCoordinatesType.Odd => (col - (col & 1)) / 2,
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
                case Direction.NorthEast:
                    degrees = 300;
                    break;
                case Direction.East:
                    degrees = 0;
                    break;
                case Direction.SouthEast:
                    degrees = 60;
                    break;
                case Direction.SouthWest:
                    degrees = 120;
                    break;
                case Direction.West:
                    degrees = 180;
                    break;
                case Direction.NorthWest:
                    degrees = 240;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return degrees;
        }
    }
}