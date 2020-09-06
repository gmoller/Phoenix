using Microsoft.Xna.Framework;
using Hex;
using Utilities;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointExtensions
    {
        public static PointI ToPointI(this Point p)
        {
            return new PointI(p.X, p.Y);
        }

        public static bool IsWithinHex(this Point p1, HexOffsetCoordinates p2, Matrix transform)
        {
            var hexPoint = p1.ToWorldHex(transform);
            var cursorIsOnHex = p2.ToPointI() == hexPoint;

            return cursorIsOnHex;
        }

        private static PointI ToWorldHex(this Point p, Matrix transform)
        {
            var worldPositionPointedAtByMouseCursor = p.ToWorldPosition(transform);
            var worldHex = HexOffsetCoordinates.FromPixel(worldPositionPointedAtByMouseCursor.X, worldPositionPointedAtByMouseCursor.Y);

            return new PointI(worldHex.Col, worldHex.Row);
        }

        private static Vector2 ToWorldPosition(this Point p, Matrix transform)
        {
            var worldPositionPointedAtByMouseCursor = Vector2.Transform(p.ToVector2(), Matrix.Invert(transform));

            return new Vector2(worldPositionPointedAtByMouseCursor.X, worldPositionPointedAtByMouseCursor.Y);
        }
    }
}