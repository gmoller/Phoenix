using Microsoft.Xna.Framework;
using Utilities;
using Point = Utilities.Point;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class Vector2Extensions
    {
        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static PointF ToPointF(this Vector2 v)
        {
            return new PointF(v.X, v.Y);
        }
    }
}