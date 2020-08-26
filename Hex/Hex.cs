using System;
using Utilities;

namespace Hex
{
    public struct Hex
    {
        public static PointF GetCorner(HexVertexDirection direction)
        {
            float degrees = 60 * (int)direction - 30;
            float radians = MathHelper.ToRadians(degrees);

            var v = new PointF((float)(Constants.HexSize * Math.Cos(radians)), (float)(Constants.HexSize * Math.Sin(radians)));

            return v;
        }
    }
}