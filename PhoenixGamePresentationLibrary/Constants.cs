using Microsoft.Xna.Framework;

namespace PhoenixGamePresentationLibrary
{
    public static class Constants
    {
        public static int WORLD_MAP_WIDTH_IN_PIXELS = PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS * (int)HexLibrary.Constants.HEX_WIDTH - (int)HexLibrary.Constants.HEX_HALF_WIDTH;
        public static int WORLD_MAP_HEIGHT_IN_PIXELS = (PhoenixGameLibrary.Constants.WORLD_MAP_ROWS / 2 * (int)HexLibrary.Constants.HEX_HEIGHT) + (PhoenixGameLibrary.Constants.WORLD_MAP_ROWS / 2 * (int)HexLibrary.Constants.HEX_SIZE) - (int)HexLibrary.Constants.HEX_HALF_HEIGHT;

        public static Vector2 HEX_ORIGIN = new Vector2(256.0f * 0.5f, 256.0f * 0.5f + 128.0f);
    }
}