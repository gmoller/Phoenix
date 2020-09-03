using Microsoft.Xna.Framework;
using Utilities;
using PointI = Utilities.PointI;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class Vector2Extensions
    {
        public static PointI ToPoint(this Vector2 v)
        {
            return new PointI((int)v.X, (int)v.Y);
        }

        public static PointF ToPointF(this Vector2 v)
        {
            return new PointF(v.X, v.Y);
        }
    }
}