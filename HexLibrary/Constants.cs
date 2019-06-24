using System;

namespace HexLibrary
{
    public static class Constants
    {
        public const float HEX_SCALE = 0.5f;

        public static double SQUARE_ROOT_OF_3 = Math.Sqrt(3);

        public static double HEX_ACTUAL_SIZE = HEX_ACTUAL_HEIGHT * 0.5f;
        public static double HEX_ACTUAL_WIDTH = SQUARE_ROOT_OF_3 * HEX_ACTUAL_SIZE; // 221
        public const float HEX_ACTUAL_HEIGHT = 256.0f;

        public static double HEX_WIDTH = HEX_ACTUAL_WIDTH * HEX_SCALE;
        public static double HEX_HALF_WIDTH = HEX_WIDTH * 0.5f;

        public const float HEX_HEIGHT = HEX_ACTUAL_HEIGHT * HEX_SCALE;
        public const float HEX_HALF_HEIGHT = HEX_HEIGHT * 0.5f;
        public const float HEX_THREE_QUARTER_HEIGHT = HEX_HEIGHT * 0.75F;

        public const float HEX_SIZE = HEX_HALF_HEIGHT;

        public const float ONE_THIRD = 1 / 3.0f;
        public const float TWO_THIRDS = 2 / 3.0f;

        public static double ONE_THIRD_OF_SQUARE_ROOT_OF_3 = SQUARE_ROOT_OF_3 / 3.0f;
        public static double HALF_OF_SQUARE_ROOT_OF_3 = Math.Sqrt(3) / 2.0f;
    }
}