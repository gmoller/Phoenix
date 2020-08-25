namespace Utilities
{
    public struct Color
    {
        public static Color Black => new Color(0, 0, 0, 255);
        public static Color ForestGreen => new Color(34, 139, 34, 255);
        public static Color LightGreen => new Color(144, 238, 144, 255);
        public static Color RoyalBlue => new Color(65, 105, 225, 255);
        public static Color SaddleBrown => new Color(139, 69, 19, 255);
        public static Color SandyBrown => new Color(244, 164, 96, 255);
        public static Color SeaGreen => new Color(46, 139, 87, 255);
        public static Color Snow => new Color(255, 250, 250, 255);
        public static Color Tan => new Color(210, 180, 140, 255);
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