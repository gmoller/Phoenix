namespace Utilities
{
    public readonly struct ColorRgba
    {
        public static ColorRgba AliceBlue => new ColorRgba(240, 248, 255, 255);
        public static ColorRgba AntiqueWhite => new ColorRgba(250, 235, 215, 255);
        public static ColorRgba Aqua => new ColorRgba(0, 255, 255, 255);
        public static ColorRgba Aquamarine => new ColorRgba(127, 255, 212, 255);
        public static ColorRgba Azure => new ColorRgba(240, 255, 255, 255);
        public static ColorRgba Beige => new ColorRgba(245, 245, 220, 255);
        public static ColorRgba Bisque => new ColorRgba(255, 228, 196, 255);
        public static ColorRgba Black => new ColorRgba(0, 0, 0, 255);
        public static ColorRgba BlanchedAlmond => new ColorRgba(255, 235, 205, 255);
        public static ColorRgba Blue => new ColorRgba(0, 0, 255, 255);
        public static ColorRgba BlueViolet => new ColorRgba(138, 43, 226, 255);
        public static ColorRgba Brown => new ColorRgba(165, 42, 42, 255);
        public static ColorRgba BurlyWood => new ColorRgba(222, 184, 135, 255);
        public static ColorRgba DarkGray => new ColorRgba(169, 169, 169, 255);
        public static ColorRgba DarkSlateBlue => new ColorRgba(72, 61, 139, 255);
        public static ColorRgba DarkSlateGray => new ColorRgba(47, 79, 79, 255);
        public static ColorRgba DimGray => new ColorRgba(105, 105, 105, 255);
        public static ColorRgba ForestGreen => new ColorRgba(34, 139, 34, 255);
        public static ColorRgba LightGreen => new ColorRgba(144, 238, 144, 255);
        public static ColorRgba MidnightBlue => new ColorRgba(25, 25, 112, 255);
        public static ColorRgba RoyalBlue => new ColorRgba(65, 105, 225, 255);
        public static ColorRgba SaddleBrown => new ColorRgba(139, 69, 19, 255);
        public static ColorRgba SandyBrown => new ColorRgba(244, 164, 96, 255);
        public static ColorRgba SeaGreen => new ColorRgba(46, 139, 87, 255);
        public static ColorRgba Snow => new ColorRgba(255, 250, 250, 255);
        public static ColorRgba Tan => new ColorRgba(210, 180, 140, 255);
        public static ColorRgba TransparentBlack => new ColorRgba(0, 0, 0, 0);
        public static ColorRgba TransparentWhite => new ColorRgba(255, 255, 255, 0);
        public static ColorRgba White => new ColorRgba(255, 255, 255, 255);

        private ColorRgba(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }
    }
}