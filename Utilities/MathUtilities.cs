namespace Utilities
{
    public static class MathUtilities
    {
        /// <summary>
        /// Converts Degrees to Radians.
        /// </summary>
        /// <param name="degrees">Degrees.</param>
        /// <returns></returns>
        public static float ToRadians(float degrees)
        {
            // This method uses double precision internally,
            // though it returns single float
            // Factor = pi / 180
            return (float)(degrees * 0.017453292519943295769236907684886);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }
    }
}