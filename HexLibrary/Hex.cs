using System;
using Microsoft.Xna.Framework;

namespace HexLibrary
{
    public struct Hex
    {
        public static Vector2 GetCorner(HexVertexDirection direction)
        {
            float degrees = 60 * (int)direction - 30;
            float radians = MathHelper.ToRadians(degrees);

            var v = new Vector2((float)(Constants.HexSize * Math.Cos(radians)), (float)(Constants.HexSize * Math.Sin(radians)));

            return v;
        }
    }
}