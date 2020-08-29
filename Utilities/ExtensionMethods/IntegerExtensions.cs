namespace Utilities.ExtensionMethods
{
    public static class IntegerExtensions
    {
        public static int ResetBit(this int i, int bitNumber)
        {
            i &= byte.MaxValue ^ (1 << bitNumber);

            return i;
        }

        public static int SetBit(this int i, int bitNumber)
        {
            i |= 1 << bitNumber;

            return i;
        }

        public static bool IsEven(this int i)
        {
            return (i % 2) == 0;
        }
    }
}