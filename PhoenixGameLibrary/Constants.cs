using Microsoft.Xna.Framework;

namespace PhoenixGameLibrary
{
    public static class Constants
    {
        public const int WORLD_MAP_COLUMNS = 60;
        public const int WORLD_MAP_ROWS = 40;

        public static int WORLD_MAP_WIDTH_IN_PIXELS = WORLD_MAP_COLUMNS * (int)HexLibrary.Constants.HEX_WIDTH - (int)HexLibrary.Constants.HEX_HALF_WIDTH;
        public static int WORLD_MAP_HEIGHT_IN_PIXELS = (WORLD_MAP_ROWS / 2 * (int)HexLibrary.Constants.HEX_HEIGHT) + (WORLD_MAP_ROWS / 2 * (int)HexLibrary.Constants.HEX_SIZE) - (int)HexLibrary.Constants.HEX_HALF_HEIGHT;

        public static Vector2 HEX_ORIGIN = new Vector2(256.0f * 0.5f, 256.0f * 0.5f + 128.0f);
    }
}