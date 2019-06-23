using Microsoft.Xna.Framework;

namespace PhoenixGameLibrary
{
    public static class Constants
    {
        public const int WORLD_MAP_WIDTH_IN_HEXES = 60;
        public const int WORLD_MAP_HEIGHT_IN_HEXES = 40;

        public const int WORLD_MAP_WIDTH_IN_PIXELS = WORLD_MAP_WIDTH_IN_HEXES * (int)HEX_WIDTH - (int)HEX_HALF_WIDTH;
        public const int WORLD_MAP_HEIGHT_IN_PIXELS = (WORLD_MAP_HEIGHT_IN_HEXES / 2 * (int)HEX_HEIGHT) + (WORLD_MAP_HEIGHT_IN_HEXES / 2 * (int)HEX_SIDE) - (int)HEX_HALF_HEIGHT;

        //----------------------------------

        public const float HEX_SCALE = 0.5f;
        public const float HEX_ACTUAL_WIDTH = 256.0f;
        public const float HEX_ACTUAL_HEIGHT = 256.0f;

        public const float HEX_WIDTH = HEX_ACTUAL_WIDTH * HEX_SCALE;
        public const float HEX_THREE_QUARTER_WIDTH = HEX_WIDTH * 0.75F;
        public const float HEX_HALF_WIDTH = HEX_WIDTH * 0.5f;

        public const float HEX_HEIGHT = HEX_ACTUAL_HEIGHT * HEX_SCALE;
        public const float HEX_THREE_QUARTER_HEIGHT = HEX_HEIGHT * 0.75F;
        public const float HEX_HALF_HEIGHT = HEX_HEIGHT * 0.5f;
        public const float HEX_ONE_QUARTER_HEIGHT = HEX_HEIGHT * 0.25F;

        public const float HEX_SIDE = 128.0f * HEX_SCALE;

        public static Vector2 HEX_ORIGIN = new Vector2(HEX_ACTUAL_WIDTH * 0.5f, HEX_ACTUAL_HEIGHT * 0.5f + 128.0f);
    }
}