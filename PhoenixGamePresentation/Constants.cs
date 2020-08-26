using Microsoft.Xna.Framework;

namespace PhoenixGamePresentation
{
    public static class Constants
    {
        public static readonly int WORLD_MAP_WIDTH_IN_PIXELS = PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS * (int)Hex.Constants.HexWidth - (int)Hex.Constants.HexHalfWidth;
        public static readonly int WORLD_MAP_HEIGHT_IN_PIXELS = (PhoenixGameLibrary.Constants.WORLD_MAP_ROWS / 2 * (int)Hex.Constants.HexHeight) + (PhoenixGameLibrary.Constants.WORLD_MAP_ROWS / 2 * (int)Hex.Constants.HexSize) - (int)Hex.Constants.HexHalfHeight;

        public static Vector2 HEX_ORIGIN = new Vector2(256.0f * ONE_HALF, 256.0f * ONE_HALF + 128.0f);

        public const float ONE_HALF = 1 / 2.0f;
    }
}