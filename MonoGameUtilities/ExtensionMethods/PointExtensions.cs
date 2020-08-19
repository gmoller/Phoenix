using Microsoft.Xna.Framework;
using Point = Utilities.Point;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointExtensions
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}