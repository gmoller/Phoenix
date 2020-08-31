namespace Utilities
{
    public readonly struct Color
    {
        public static Color AliceBlue => new Color(240, 248, 255, 255);
        public static Color AntiqueWhite => new Color(250, 235, 215, 255);
        public static Color Aqua => new Color(0, 255, 255, 255);
        public static Color Aquamarine => new Color(127, 255, 212, 255);
        public static Color Azure => new Color(240, 255, 255, 255);
        public static Color Beige => new Color(245, 245, 220, 255);
        public static Color Bisque => new Color(255, 228, 196, 255);
        public static Color Black => new Color(0, 0, 0, 255);
        public static Color BlanchedAlmond => new Color(255, 235, 205, 255);
        public static Color Blue => new Color(0, 0, 255, 255);
        public static Color BlueViolet => new Color(138, 43, 226, 255);
        public static Color Brown => new Color(165, 42, 42, 255);
        public static Color BurlyWood => new Color(222, 184, 135, 255);
        public static Color DarkGray => new Color(169, 169, 169, 255);
        public static Color DarkSlateBlue => new Color(72, 61, 139, 255);
        public static Color DarkSlateGray => new Color(47, 79, 79, 255);
        public static Color DimGray => new Color(105, 105, 105, 255);
        public static Color ForestGreen => new Color(34, 139, 34, 255);
        public static Color LightGreen => new Color(144, 238, 144, 255);
        public static Color MidnightBlue => new Color(25, 25, 112, 255);
        public static Color RoyalBlue => new Color(65, 105, 225, 255);
        public static Color SaddleBrown => new Color(139, 69, 19, 255);
        public static Color SandyBrown => new Color(244, 164, 96, 255);
        public static Color SeaGreen => new Color(46, 139, 87, 255);
        public static Color Snow => new Color(255, 250, 250, 255);
        public static Color Tan => new Color(210, 180, 140, 255);
        public static Color TransparentBlack => new Color(0, 0, 0, 0);
        public static Color TransparentWhite => new Color(255, 255, 255, 0);
        public static Color White => new Color(255, 255, 255, 255);

        public Color(byte r, byte g, byte b, byte a)
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