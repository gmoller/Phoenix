using System.Collections.Generic;
using Utilities;

namespace Hex
{
    public interface IHex
    {
        HexCube OffsetCoordinatesToCube(HexOffsetCoordinates hexOffsetCoordinates);
        HexAxial OffsetCoordinatesToAxial(HexOffsetCoordinates offsetCoordinates);
        HexOffsetCoordinates CubeToOffsetCoordinates(HexCube hexCube);
        HexAxial CubeToAxial(HexCube hexCube);
        HexOffsetCoordinates AxialToOffsetCoordinates(HexAxial hexAxial);
        HexCube AxialToCube(HexAxial hexAxial);

        HexCube RoundCube(float x, float y, float z);
        HexAxial RoundAxial(float q, float r);
        HexOffsetCoordinates RoundOffsetCoordinates(float x, float y);

        HexOffsetCoordinates[] GetAllNeighbors(HexOffsetCoordinates hexOffsetCoordinates);
        HexOffsetCoordinates GetNeighbor(HexOffsetCoordinates hexOffsetCoordinates, Direction direction);
        HexOffsetCoordinates[] GetSingleRing(HexOffsetCoordinates offsetCoordinates, int radius);
        HexOffsetCoordinates[] GetSpiralRing(HexOffsetCoordinates offsetCoordinates, int radius);
        List<HexOffsetCoordinates> GetLine(HexOffsetCoordinates fromOffsetCoordinates, HexOffsetCoordinates toOffsetCoordinates);
        int GetDistance(HexOffsetCoordinates from, HexOffsetCoordinates to);
        HexOffsetCoordinates FromPixelToOffsetCoordinates(int x, int y);
        PointF FromOffsetCoordinatesToPixel(HexOffsetCoordinates offsetCoordinates);

        HexCube[] GetAllNeighbors(HexCube hexCube);
        HexCube GetNeighbor(HexCube hexCube, Direction direction);
        List<HexCube> GetLine(HexCube fromCube, HexCube toCube);
        int GetDistance(HexCube fromCube, HexCube toCube);
        HexCube FromPixelToCube(int x, int y);
        PointF FromCubeToPixel(HexCube cube);

        HexAxial FromPixelToAxial(int x, int y);
        PointF FromAxialToPixel(HexAxial axial);

        (float x, float y, float z) Lerp(HexCube a, HexCube b, float t);

        PointF GetCorner(Direction direction);
    }
}