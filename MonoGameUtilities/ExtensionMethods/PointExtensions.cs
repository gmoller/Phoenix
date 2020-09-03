using Microsoft.Xna.Framework;
using PointI = Utilities.PointI;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointExtensions
    {
        public static Vector2 ToVector2(this PointI p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}