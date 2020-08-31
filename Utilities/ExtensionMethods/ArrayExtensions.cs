namespace Utilities.ExtensionMethods
{
    public static class ArrayExtensions
    {
        public static T[] To1DArray<T>(this T[,] array)
        {
            var size = array.Length;
            var result = new T[size];

            var index = 0;
            for (var i = 0; i <= array.GetUpperBound(1); i++)
            {
                for (var j = 0; j <= array.GetUpperBound(0); j++)
                {
                    result[index++] = array[j, i];
                }
            }

            return result;
        }
    }
}