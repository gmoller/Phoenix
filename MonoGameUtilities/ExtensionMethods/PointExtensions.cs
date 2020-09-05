using Microsoft.Xna.Framework;
using Utilities;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointExtensions
    {
        public static PointI ToPointI(this Point p)
        {
            return new PointI(p.X, p.Y);
        }
    }
}