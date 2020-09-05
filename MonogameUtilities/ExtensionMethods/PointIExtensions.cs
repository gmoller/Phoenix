using Microsoft.Xna.Framework;
using Utilities;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointIExtensions
    {
        public static Vector2 ToVector2(this PointI p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}