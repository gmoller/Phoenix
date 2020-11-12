namespace Utilities
{
    //http://www.foszor.com/blog/xna-color-chart/
    //https://cloford.com/resources/colours/500col.htm

    public readonly struct ColorRgba
    {
        public static ColorRgba AliceBlue => new ColorRgba(240, 248, 255, 255);
        public static ColorRgba AntiqueWhite => new ColorRgba(250, 235, 215, 255);
        public static ColorRgba AntiqueWhite1 => new ColorRgba(255, 239, 219, 255);
        public static ColorRgba AntiqueWhite2 => new ColorRgba(238, 223, 204, 255);
        public static ColorRgba AntiqueWhite3 => new ColorRgba(205, 192, 176, 255);
        public static ColorRgba AntiqueWhite4 => new ColorRgba(139, 131, 120, 255);
        public static ColorRgba Aqua => new ColorRgba(0, 255, 255, 255); // cyan
        public static ColorRgba Aquamarine => new ColorRgba(127, 255, 212, 255); // aquamarine1
        public static ColorRgba Aquamarine2 => new ColorRgba(118, 238, 198, 255);
        public static ColorRgba Aquamarine3 => new ColorRgba(102, 205, 170, 255); // mediumaquamarine
        public static ColorRgba Aquamarine4 => new ColorRgba(69, 139, 116, 255);
        public static ColorRgba Azure => new ColorRgba(240, 255, 255, 255); // azure1
        public static ColorRgba Azure2 => new ColorRgba(224, 238, 238, 255);
        public static ColorRgba Azure3 => new ColorRgba(193, 205, 205, 255);
        public static ColorRgba Azure4 => new ColorRgba(131, 139, 139, 255);
        public static ColorRgba Beige => new ColorRgba(245, 245, 220, 255);
        public static ColorRgba Bisque => new ColorRgba(255, 228, 196, 255); // bisque1
        public static ColorRgba Bisque2 => new ColorRgba(238, 213, 183, 255);
        public static ColorRgba Bisque3 => new ColorRgba(205, 183, 158, 255);
        public static ColorRgba Bisque4 => new ColorRgba(139, 125, 107, 255);
        public static ColorRgba Black => new ColorRgba(0, 0, 0, 255);
        public static ColorRgba BlanchedAlmond => new ColorRgba(255, 235, 205, 255);
        public static ColorRgba Blue => new ColorRgba(0, 0, 255, 255);
        public static ColorRgba Blue2 => new ColorRgba(0, 0, 238, 255);
        public static ColorRgba Blue3 => new ColorRgba(0, 0, 205, 255); // mediumblue
        public static ColorRgba Blue4 => new ColorRgba(0, 0, 139, 255); // darkblue
        public static ColorRgba BlueViolet => new ColorRgba(138, 43, 226, 255);
        public static ColorRgba Brown => new ColorRgba(165, 42, 42, 255);
        public static ColorRgba Brown1 => new ColorRgba(255, 64, 64, 255);
        public static ColorRgba Brown2 => new ColorRgba(238, 59, 59, 255);
        public static ColorRgba Brown3 => new ColorRgba(205, 51, 51, 255);
        public static ColorRgba Brown4 => new ColorRgba(139, 35, 35, 255);
        public static ColorRgba BurlyWood => new ColorRgba(222, 184, 135, 255);
        public static ColorRgba BurlyWood1 => new ColorRgba(255, 211, 155, 255);
        public static ColorRgba BurlyWood2 => new ColorRgba(238, 197, 145, 255);
        public static ColorRgba BurlyWood3 => new ColorRgba(205, 170, 125, 255);
        public static ColorRgba BurlyWood4 => new ColorRgba(139, 115, 85, 255);
        public static ColorRgba Cyan => new ColorRgba(0, 255, 255, 255);
        public static ColorRgba Cyan2 => new ColorRgba(0, 238, 238, 255);
        public static ColorRgba Cyan3 => new ColorRgba(0, 205, 205, 255);
        public static ColorRgba Cyan4 => new ColorRgba(0, 139, 139, 255);
        public static ColorRgba DarkGray => new ColorRgba(169, 169, 169, 255);
        public static ColorRgba DarkSlateBlue => new ColorRgba(72, 61, 139, 255);
        public static ColorRgba DarkSlateGray => new ColorRgba(47, 79, 79, 255);
        public static ColorRgba DarkSlateGray1 => new ColorRgba(151, 255, 255, 255);
        public static ColorRgba DarkSlateGray2 => new ColorRgba(141, 238, 238, 255);
        public static ColorRgba DarkSlateGray3 => new ColorRgba(121, 205, 205, 255);
        public static ColorRgba DarkSlateGray4 => new ColorRgba(82, 139, 139, 255);
        public static ColorRgba DimGray => new ColorRgba(105, 105, 105, 255); // gray42
        public static ColorRgba ForestGreen => new ColorRgba(34, 139, 34, 255);
        public static ColorRgba LightGreen => new ColorRgba(144, 238, 144, 255); // palegreen2
        public static ColorRgba MidnightBlue => new ColorRgba(25, 25, 112, 255);
        public static ColorRgba RoyalBlue => new ColorRgba(65, 105, 225, 255);
        public static ColorRgba RoyalBlue1 => new ColorRgba(72, 118, 225, 255);
        public static ColorRgba RoyalBlue2 => new ColorRgba(67, 110, 238, 255);
        public static ColorRgba RoyalBlue3 => new ColorRgba(58, 95, 205, 255);
        public static ColorRgba RoyalBlue4 => new ColorRgba(39, 64, 139, 255);
        public static ColorRgba SaddleBrown => new ColorRgba(139, 69, 19, 255); // chocolate4
        public static ColorRgba SandyBrown => new ColorRgba(244, 164, 96, 255);
        public static ColorRgba SeaGreen => new ColorRgba(46, 139, 87, 255); // seagreen4
        public static ColorRgba SeaGreen1 => new ColorRgba(84, 255, 159, 255);
        public static ColorRgba SeaGreen2 => new ColorRgba(78, 238, 148, 255);
        public static ColorRgba SeaGreen3 => new ColorRgba(67, 205, 128, 255);
        public static ColorRgba Snow => new ColorRgba(255, 250, 250, 255); // snow1
        public static ColorRgba Snow2 => new ColorRgba(238, 233, 233, 255);
        public static ColorRgba Snow3 => new ColorRgba(205, 201, 201, 255);
        public static ColorRgba Snow4 => new ColorRgba(139, 137, 137, 255);
        public static ColorRgba Tan => new ColorRgba(210, 180, 140, 255);
        public static ColorRgba Tan1 => new ColorRgba(255, 165, 79, 255);
        public static ColorRgba Tan2 => new ColorRgba(238, 154, 73, 255);
        public static ColorRgba Tan3 => new ColorRgba(205, 133, 63, 255); // peru
        public static ColorRgba Tan4 => new ColorRgba(139, 90, 43, 255);
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