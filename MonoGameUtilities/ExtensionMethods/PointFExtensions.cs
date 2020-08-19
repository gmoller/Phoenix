using Microsoft.Xna.Framework;
using Utilities;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointFExtensions
    {
        public static Vector2 ToVector2(this PointF p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}