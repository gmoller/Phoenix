using Microsoft.Xna.Framework;
using Zen.Hexagons;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class Point2FExtensions
    {
        public static Vector2 ToVector2(this Point2F p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}