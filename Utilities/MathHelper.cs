namespace Utilities
{
    public static class MathHelper
    {
        public static float ToRadians(float degrees)
        {
            // This method uses double precision internally,
            // though it returns single float
            // Factor = pi / 180
            return (float)(degrees * 0.017453292519943295769236907684886);
        }
    }
}