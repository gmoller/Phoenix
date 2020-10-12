﻿using System;
using System.Collections.Generic;
using Utilities;

namespace Hex
{
    public class HexLibrary : IHex
    {
        private readonly IHex _hex;

        public HexLibrary(HexType hexType, OffsetCoordinatesType offsetCoordinatesType)
        {
            _hex = hexType switch
            {
                HexType.FlatTopped => new HexFlatTopped(offsetCoordinatesType),
                HexType.PointyTopped => new HexPointyTopped(offsetCoordinatesType),
                _ => throw new ArgumentOutOfRangeException(nameof(hexType), hexType, $"HexType {hexType} is not supported.")
            };
        }

        public HexCube OffsetCoordinatesToCube(HexOffsetCoordinates hexOffsetCoordinates) => _hex.OffsetCoordinatesToCube(hexOffsetCoordinates);
        public HexAxial OffsetCoordinatesToAxial(HexOffsetCoordinates offsetCoordinates) => _hex.OffsetCoordinatesToAxial(offsetCoordinates);
        public HexOffsetCoordinates CubeToOffsetCoordinates(HexCube hexCube) => _hex.CubeToOffsetCoordinates(hexCube);
        public HexAxial CubeToAxial(HexCube hexCube) => _hex.CubeToAxial(hexCube);
        public HexOffsetCoordinates AxialToOffsetCoordinates(HexAxial hexAxial) => _hex.AxialToOffsetCoordinates(hexAxial);
        public HexCube AxialToCube(HexAxial hexAxial) => _hex.AxialToCube(hexAxial);

        public HexCube RoundCube(float x, float y, float z) => _hex.RoundCube(x, y, z);
        public HexAxial RoundAxial(float q, float r) => _hex.RoundAxial(q, r);
        public HexOffsetCoordinates RoundOffsetCoordinates(float x, float y) => _hex.RoundOffsetCoordinates(x, y);

        public HexOffsetCoordinates[] GetAllNeighbors(HexOffsetCoordinates hexOffsetCoordinates) => _hex.GetAllNeighbors(hexOffsetCoordinates);
        public HexOffsetCoordinates GetNeighbor(HexOffsetCoordinates hexOffsetCoordinates, Direction direction) => _hex.GetNeighbor(hexOffsetCoordinates, direction);
        public HexOffsetCoordinates[] GetSingleRing(HexOffsetCoordinates offsetCoordinates, int radius) => _hex.GetSingleRing(offsetCoordinates, radius);
        public HexOffsetCoordinates[] GetSpiralRing(HexOffsetCoordinates offsetCoordinates, int radius) => _hex.GetSpiralRing(offsetCoordinates, radius);
        public List<HexOffsetCoordinates> GetLine(HexOffsetCoordinates fromOffsetCoordinates, HexOffsetCoordinates toOffsetCoordinates) => _hex.GetLine(fromOffsetCoordinates, toOffsetCoordinates);
        public int GetDistance(HexOffsetCoordinates from, HexOffsetCoordinates to) => _hex.GetDistance(from, to);
        public HexOffsetCoordinates FromPixelToOffsetCoordinates(int x, int y) => _hex.FromPixelToOffsetCoordinates(x, y);
        public PointF FromOffsetCoordinatesToPixel(HexOffsetCoordinates offsetCoordinates) => _hex.FromOffsetCoordinatesToPixel(offsetCoordinates);

        public HexCube[] GetAllNeighbors(HexCube hexCube) => _hex.GetAllNeighbors(hexCube);
        public HexCube GetNeighbor(HexCube hexCube, Direction direction) => _hex.GetNeighbor(hexCube, direction);
        public List<HexCube> GetLine(HexCube fromCube, HexCube toCube) => _hex.GetLine(fromCube, toCube);
        public int GetDistance(HexCube fromCube, HexCube toCube) => _hex.GetDistance(fromCube, toCube);
        public HexCube FromPixelToCube(int x, int y) => _hex.FromPixelToCube(x, y);
        public PointF FromCubeToPixel(HexCube cube) => _hex.FromCubeToPixel(cube);

        public HexAxial FromPixelToAxial(int x, int y) => _hex.FromPixelToAxial(x, y);
        public PointF FromAxialToPixel(HexAxial axial) => _hex.FromAxialToPixel(axial);

        public  (float x, float y, float z) Lerp(HexCube a, HexCube b, float t) => _hex.Lerp(a, b, t);

        public PointF GetCorner(Direction direction) => _hex.GetCorner(direction);
    }
}