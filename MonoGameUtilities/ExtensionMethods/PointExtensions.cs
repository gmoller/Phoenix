using Microsoft.Xna.Framework;
using Hex;
using Utilities;

namespace MonoGameUtilities.ExtensionMethods
{
    public static class PointExtensions
    {
        public static PointI ToPointI(this Point p)
        {
            var point = new PointI(p.X, p.Y);

            return point;
        }

        public static bool IsWithinRectangle(this Point p1, Rectangle rectangle)
        {
            var isWithinRectangle = rectangle.Contains(p1);

            return isWithinRectangle;
        }

        public static bool IsWithinHex(this Point p1, PointI p2, Matrix transform)
        {
            var isWithinHex = p1.IsWithinHex(new HexOffsetCoordinates(p2), transform);

            return isWithinHex;
        }

        public static bool IsWithinHex(this Point p1, HexOffsetCoordinates p2, Matrix transform)
        {
            var hexPoint = p1.ToWorldHex(transform);
            var isWithinHex = p2.ToPointI() == hexPoint;

            return isWithinHex;
        }

        public static PointI ToWorldHex(this Point p, Matrix transform)
        {
            var worldPositionPointedAtByMouseCursor = p.ToWorldPosition(transform);
            var hexLibrary = new HexLibrary(HexType.PointyTopped, OffsetCoordinatesType.Odd);
            var worldHex = hexLibrary.FromPixelToOffsetCoordinates((int)worldPositionPointedAtByMouseCursor.X, (int)worldPositionPointedAtByMouseCursor.Y);
            var worldHexPoint = new PointI(worldHex.Col, worldHex.Row);

            return worldHexPoint;
        }

        public static Vector2 ToWorldPosition(this Point p, Matrix transform)
        {
            var worldPositionPointedAtByMouseCursor = Vector2.Transform(p.ToVector2(), Matrix.Invert(transform));
            var worldPositionPointedAtByMouseCursorVector = new Vector2(worldPositionPointedAtByMouseCursor.X, worldPositionPointedAtByMouseCursor.Y);

            return worldPositionPointedAtByMouseCursorVector;
        }
    }
}