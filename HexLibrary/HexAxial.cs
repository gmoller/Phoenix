﻿using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace HexLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct HexAxial
    {
        public int Q { get; }
        public int R { get; }

        public HexAxial(int q, int r)
        {
            Q = q;
            R = r;
        }

        public static HexOffsetCoordinates ToOffsetCoordinates(HexAxial axial)
        {
            return ToOffsetCoordinates(axial.Q, axial.R);
        }

        public static HexOffsetCoordinates ToOffsetCoordinates(int q, int r)
        {
            var cube = ToCube(q, r);
            var offsetCoordinates = HexCube.ToOffsetCoordinates(cube);

            return offsetCoordinates;
        }

        public static HexCube ToCube(HexAxial axial)
        {
            return ToCube(axial.Q, axial.R);
        }

        public static HexCube ToCube(int q, int r)
        {
            var x = q;
            var z = r;
            var y = -x - z;
            var cube = new HexCube(x, y, z);

            return cube;
        }

        public static HexAxial Round(HexAxial axial)
        {
            return Round(axial.Q, axial.R);
        }

        public static HexAxial Round(float q, float r)
        {
            var cube = HexCube.Round(q, -q - r, r);
            var axial = HexCube.ToAxial(cube);

            return axial;
        }

        public static Vector2 ToPixel(HexAxial axial)
        {
            return ToPixel(axial.Q, axial.R);
        }

        public static Vector2 ToPixel(int q, int r)
        {
            var x = Constants.HEX_SIZE * (Constants.SQUARE_ROOT_OF_3 * q + Constants.HALF_OF_SQUARE_ROOT_OF_3 * r);
            var y = Constants.HEX_SIZE * (1.5f * r);
            var pixel = new Vector2((float)x, y);

            return pixel;
        }

        public static HexAxial FromPixel(HexAxial axial)
        {
            return FromPixel(axial.Q, axial.R);
        }

        public static HexAxial FromPixel(int x, int y)
        {
            var q = (Constants.ONE_THIRD_OF_SQUARE_ROOT_OF_3 * x - Constants.ONE_THIRD * y) / Constants.HEX_SIZE;
            var r = (Constants.TWO_THIRDS * y) / Constants.HEX_SIZE;
            var axial = Round((float)q, r);

            return axial;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Q={Q},R={R}}}";
    }
}