using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Hex
{
    public abstract class Hex : IHex
    {
        protected readonly OffsetCoordinatesType OffsetCoordinatesType;

        protected Hex(OffsetCoordinatesType offsetCoordinatesType)
        {
            OffsetCoordinatesType = offsetCoordinatesType;
        }


        protected abstract HexCube GetNeighboringCube(Direction direction);


        public abstract HexCube OffsetCoordinatesToCube(HexOffsetCoordinates hexOffsetCoordinates);

        public HexAxial OffsetCoordinatesToAxial(HexOffsetCoordinates offsetCoordinates)
        {
            var cube = OffsetCoordinatesToCube(offsetCoordinates);
            var axial = CubeToAxial(cube);

            return axial;
        }

        public abstract HexOffsetCoordinates CubeToOffsetCoordinates(HexCube hexCube);

        public HexAxial CubeToAxial(HexCube hexCube)
        {
            var q = hexCube.X;
            var r = hexCube.Z;
            var axial = new HexAxial(q, r);

            return axial;
        }

        public HexOffsetCoordinates AxialToOffsetCoordinates(HexAxial hexAxial)
        {
            var cube = AxialToCube(hexAxial);
            var offsetCoordinates = CubeToOffsetCoordinates(cube);

            return offsetCoordinates;
        }

        public HexCube AxialToCube(HexAxial hexAxial)
        {
            var x = hexAxial.Q;
            var z = hexAxial.R;
            var y = -x - z;
            var cube = new HexCube(x, y, z);

            return cube;
        }


        public HexCube RoundCube(float x, float y, float z)
        {
            var rx = (int)Math.Round(x);
            var ry = (int)Math.Round(y);
            var rz = (int)Math.Round(z);

            //var zero = rx + ry + rz;

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

        public HexAxial RoundAxial(float q, float r)
        {
            var cube = RoundCube(q, -q - r, r);
            var axial = CubeToAxial(cube);

            return axial;
        }

        public HexOffsetCoordinates RoundOffsetCoordinates(float x, float y)
        {
            var cube = RoundCube(x, -x - y, y);
            var offsetCoordinates = CubeToOffsetCoordinates(cube);

            return offsetCoordinates;
        }


        public HexOffsetCoordinates[] GetAllNeighbors(HexOffsetCoordinates hexOffsetCoordinates)
        {
            var cube = OffsetCoordinatesToCube(hexOffsetCoordinates);
            var allNeighboringCubes = GetAllNeighbors(cube);

            var neighbors = new HexOffsetCoordinates[6];
            for (var i = 0; i < 6; i++)
            {
                var neighboringCube = allNeighboringCubes[i];
                var neighboring = CubeToOffsetCoordinates(neighboringCube);
                neighbors[i] = neighboring;
            }

            return neighbors;
        }

        public HexOffsetCoordinates GetNeighbor(HexOffsetCoordinates hexOffsetCoordinates, Direction direction)
        {
            var cube = OffsetCoordinatesToCube(hexOffsetCoordinates);
            var neighbor = GetNeighbor(cube, direction);
            var offsetCoordinates = CubeToOffsetCoordinates(neighbor);

            return offsetCoordinates;
        }

        public HexOffsetCoordinates[] GetSingleRing(HexOffsetCoordinates offsetCoordinates, int radius)
        {
            var ring = new List<HexOffsetCoordinates>();

            var cube = OffsetCoordinatesToCube(offsetCoordinates);
            var westNeighbor = GetNeighboringCube(Direction.West);
            var scaledCube = westNeighbor * radius;
            cube += scaledCube;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < radius; j++)
                {
                    var offset = CubeToOffsetCoordinates(cube);
                    if (!ring.Contains(offset))
                    {
                        ring.Add(offset);
                    }
                    cube = GetNeighbor(cube, (Direction)i);
                }
            }
            var singleRing = ring.ToArray();

            return singleRing;
        }

        public HexOffsetCoordinates[] GetSpiralRing(HexOffsetCoordinates offsetCoordinates, int radius)
        {
            var ring = new List<HexOffsetCoordinates> { offsetCoordinates };

            for (var k = 1; k <= radius; k++)
            {
                var singleRing = GetSingleRing(offsetCoordinates, k);
                ring.AddRange(singleRing.ToList());
            }
            var spiralRing = ring.ToArray();

            return spiralRing;
        }

        public List<HexOffsetCoordinates> GetLine(HexOffsetCoordinates fromOffsetCoordinates, HexOffsetCoordinates toOffsetCoordinates)
        {
            var fromCube = OffsetCoordinatesToCube(fromOffsetCoordinates);
            var toCube = OffsetCoordinatesToCube(toOffsetCoordinates);

            var hexes = GetLine(fromCube, toCube);

            var result = new List<HexOffsetCoordinates>();
            foreach (var hex in hexes)
            {
                var offsetCoordinates = CubeToOffsetCoordinates(hex);
                result.Add(offsetCoordinates);
            }

            return result;
        }

        public int GetDistance(HexOffsetCoordinates from, HexOffsetCoordinates to)
        {
            var fromCube = OffsetCoordinatesToCube(from);
            var toCube = OffsetCoordinatesToCube(to);

            var distance = GetDistance(fromCube, toCube);

            return distance;
        }

        public HexOffsetCoordinates FromPixelToOffsetCoordinates(int x, int y)
        {
            var cube = FromPixelToCube(x, y);
            var offsetCoordinates2 = CubeToOffsetCoordinates(cube);

            return offsetCoordinates2;
        }

        public PointF ToPixel(HexOffsetCoordinates offsetCoordinates)
        {
            // TODO: should this change for flat topped?
            var x = Constants.HexSize * (Constants.SquareRootOf3 * (offsetCoordinates.Col + 0.5f * (offsetCoordinates.Row & 1)));
            var y = Constants.HexSize * (1.5f * offsetCoordinates.Row);
            var pixel = new PointF((float)x, (float)y);

            return pixel;
        }

        public HexCube[] GetAllNeighbors(HexCube hexCube)
        {
            var neighbors = new List<HexCube>();
            for (var i = 0; i < 8; i++)
            {
                var neighbor = GetNeighbor(hexCube, (Direction)i);
                if (neighbor != hexCube)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors.ToArray();
        }

        public HexCube GetNeighbor(HexCube hexCube, Direction direction)
        {
            var offset = GetNeighboringCube(direction);
            var neighbor = hexCube + offset;

            return neighbor;
        }

        public List<HexCube> GetLine(HexCube fromCube, HexCube toCube)
        {
            var distance = GetDistance(fromCube, toCube);

            var results = new List<HexCube>();
            for (var i = 0; i <= distance; i++)
            {
                var t = 1.0f / distance * i;
                var lerp = Lerp(fromCube, toCube, t);
                var hex = RoundCube(lerp.x, lerp.y, lerp.z);
                results.Add(hex);
            }

            return results;
        }

        public int GetDistance(HexCube fromCube, HexCube toCube)
        {
            var diffX = Math.Abs(fromCube.X - toCube.X);
            var diffY = Math.Abs(fromCube.Y - toCube.Y);
            var diffZ = Math.Abs(fromCube.Z - toCube.Z);
            var distance1 = (diffX + diffY + diffZ) / 2;
            var distance2 = Math.Max(Math.Max(diffX, diffY), diffZ);

            if (distance1 != distance2) throw new Exception("distance1 not equal to distance2!");

            return distance1;
        }

        public HexCube FromPixelToCube(int x, int y)
        {
            var q = (Constants.OneThirdOfSquareRootOf3 * x - Constants.ONE_THIRD * y) / Constants.HexSize;
            var r = (Constants.TWO_THIRDS * y) / Constants.HexSize;
            var axial = RoundAxial((float)q, (float)r);
            var cube = AxialToCube(axial);

            return cube;
        }

        public PointF ToPixel(HexCube cube)
        {
            var axial = CubeToAxial(cube);
            var pixel = ToPixel(axial);

            return pixel;
        }


        public HexAxial FromPixelToAxial(int x, int y)
        {
            var q = (Constants.OneThirdOfSquareRootOf3 * x - Constants.ONE_THIRD * y) / Constants.HexSize;
            var r = (Constants.TWO_THIRDS * y) / Constants.HexSize;
            var axial = RoundAxial((float)q, (float)r);

            return axial;
        }

        public PointF ToPixel(HexAxial axial)
        {
            var x = Constants.HexSize * (Constants.SquareRootOf3 * axial.Q + Constants.HalfOfSquareRootOf3 * axial.R);
            var y = Constants.HexSize * (1.5f * axial.R);
            var pixel = new PointF((float)x, (float)y);

            return pixel;
        }


        public (float x, float y, float z) Lerp(HexCube a, HexCube b, float t)
        {
            var x = MathUtilities.Lerp(a.X, b.X, t);
            var y = MathUtilities.Lerp(a.Y, b.Y, t);
            var z = MathUtilities.Lerp(a.Z, b.Z, t);

            return (x, y, z);
        }

        public PointF GetCorner(Direction direction)
        {
            var degrees = GetDegreesForHexCorner(direction);
            var radians = MathUtilities.ToRadians(degrees);
            var v = new PointF((float)(Constants.HexSize * Math.Cos(radians)), (float)(Constants.HexSize * Math.Sin(radians)));

            return v;
        }

        protected abstract float GetDegreesForHexCorner(Direction direction);
    }
}