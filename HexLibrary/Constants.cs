using System;

namespace HexLibrary
{
    // https://hexagoncalculator.apphb.com/
    // https://www.redblobgames.com/grids/hexagons/

    //    Width == Side to Side
    //  /--------\
    //      ..         \
    //    .    .       |
    //  .        .     |
    //  .        .     |
    //  .        .     | Height == Vertex to Vertex
    //  .        .     | Size == Half of Height == Center to Vertex
    //  .        .     |
    //    .    .       |
    //      ..         /

    public static class Constants
    {
        public const double ONE_HALF = 1 / 2.0f;
        public const double ONE_THIRD = 1 / 3.0f;
        public const double TWO_THIRDS = 2 / 3.0f;
        public const double THREE_QUARTERS = 3 / 4.0f;
        public static readonly double SquareRootOf3 = Math.Sqrt(3);
        public static readonly double OneThirdOfSquareRootOf3 = SquareRootOf3 / 3.0f;
        public static readonly double HalfOfSquareRootOf3 = (Math.Sqrt(3) / 2.0f);

        public const double HEX_SCALE = ONE_HALF;

        public static readonly double HexActualHeight = 256.0f;
        public static readonly double HexActualSize = HexActualHeight * 0.5f; // 128.0f
        public static readonly double HexActualWidth = SquareRootOf3 * HexActualSize; // 221.7025f

        public static readonly double HexApothem = HexActualWidth * ONE_HALF; // 110.8513f
        public static readonly double HexSideToSide = HexActualWidth; // 221.7025f
        public static readonly double HexSideLength = HexActualSize; // 128.0f
        public static readonly double HexPerimeter = 6 * HexSideLength; // 768.0f
        public static readonly double HexCenterToVertex = HexActualHeight * ONE_HALF; // 128.0f
        public static readonly double HexVertexToVertex = HexActualHeight; // 256.0f

        // scaled:
        public static readonly double HexHeight = HexActualHeight * HEX_SCALE; // 128.0f
        public static readonly double HexThreeQuarterHeight = HexHeight * THREE_QUARTERS;
        public static readonly double HexHalfHeight = HexHeight * ONE_HALF; // 64.0f
        public static readonly double HexWidth = HexActualWidth * HEX_SCALE; // 110.85125f
        public static readonly double HexHalfWidth = HexWidth * ONE_HALF; // 55.425625f

        public static readonly double HexSize = HexHalfHeight; // 64.0f
    }
}