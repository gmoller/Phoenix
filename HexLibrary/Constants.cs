using System;

namespace HexLibrary
{
    public static class Constants
    {
        public const float HEX_SCALE = 0.5f;

        public const float HEX_SIZE_X = 147.8f * HEX_SCALE;
        public const float HEX_SIZE_Y = 128.0f * HEX_SCALE;

        public const float ONE_THIRD = 1 / 3.0f;
        public const float TWO_THIRDS = 2 / 3.0f;

        public static double SQUARE_ROOT_OF_3 = Math.Sqrt(3);
        public static double ONE_THIRD_OF_SQUARE_ROOT_OF_3 = SQUARE_ROOT_OF_3 / 3.0f;
        public static double HALF_OF_SQUARE_ROOT_OF_3 = Math.Sqrt(3) / 2.0f;
    }
}