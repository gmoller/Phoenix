using System;

namespace Utilities.ExtensionMethods
{
    public static class FloatExtensions
    {
        public static bool AboutEquals(this float value1, float value2)
        {
            var epsilon = Math.Max(Math.Abs(value1), Math.Abs(value2)) * 1E-15;

            return Math.Abs(value1 - value2) <= epsilon;
        }

        public static int Round(this float value)
        {
            return (int)Math.Round(value);
        }
    }
}