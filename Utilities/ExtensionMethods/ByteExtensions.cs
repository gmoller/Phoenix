namespace Utilities.ExtensionMethods
{
    public static class ByteExtensions
    {
        public static bool IsBitSet(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }
    }
}