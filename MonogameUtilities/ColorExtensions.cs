using Microsoft.Xna.Framework;

namespace MonogameUtilities
{
    public static class ColorExtensions
    {
        public static Color ToMonogameColor(this Utilities.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}