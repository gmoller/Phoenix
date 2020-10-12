using System;

namespace Hex
{
    // https://hexagoncalculator.apphb.com/
    // https://www.redblobgames.com/grids/hexagons/

    //    Width == Side to Side (110.8513)                                       Width == Vertex to Vertex (128)
    //  /--------\                                                             /------------\
    //      ..         \                                                       
    //    .    .       |                                                          ........        \
    //  .        .     |                                                         .        .       |
    //  .        .     |                                                        .          .      |
    //  .        .     | Height == Vertex to Vertex (128)                      .            .     | Height == Side to Side (110.8513)
    //  .        .     | Size == Half of Height == Center to Vertex (64)        .          .      | Size == Half of Height == Apothem (55.4256)
    //  .        .     |                                                         .        .       |
    //    .    .       |                                                          ........        /
    //      ..         /                                                          

    public static class Constants
    {
        public const double ONE_HALF = 1 / 2.0f;
        public const double ONE_THIRD = 1 / 3.0f;
        public const double TWO_THIRDS = 2 / 3.0f;
        public const double ONE_QUARTER = 1 / 4.0f;
        //public const double THREE_QUARTERS = 3 / 4.0f;
        public static readonly double SquareRootOf3 = Math.Sqrt(3);
        public static readonly double OneThirdOfSquareRootOf3 = SquareRootOf3 / 3.0f;
        public static readonly double HalfOfSquareRootOf3 = (Math.Sqrt(3) / 2.0f);

        //private static readonly double HexActualSize = 128.0f;
        //private static readonly double HexActualWidth = SquareRootOf3 * HexActualSize; // 221.7025f

        //public static readonly double HexApothem = HexActualWidth * ONE_HALF; // 110.8513f
        //public static readonly double HexSideToSide = HexActualWidth; // 221.7025f
        //private static readonly double HexSideLength = HexActualSize; // 128.0f
        //public static readonly double HexPerimeter = 6 * HexSideLength; // 768.0f
        //public static readonly double HexCenterToVertex = 128.0f;
        //public static readonly double HexVertexToVertex = 256.0f;

        // scaled:
        public static readonly double HexHeight = 128.0f; // 110.85125f
        public static readonly double HexWidth = 110.85125f; // 128.0f
        public static readonly double HexHalfHeight = HexHeight * ONE_HALF; // 55.425625f
        public static readonly double HexQuarterHeight = HexHeight * ONE_QUARTER; // 55.425625f / 2.0f
        public static readonly double HexHalfWidth = HexWidth * ONE_HALF; // 55.425625f

        public static readonly double HexSize = HexHalfHeight; // 55.425625f
    }
}