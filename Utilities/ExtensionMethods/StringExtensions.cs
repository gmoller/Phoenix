namespace Utilities.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool HasValue(this string str)
        {
            var hasValue = !string.IsNullOrEmpty(str);

            return hasValue;
        }

        public static string KeepOnlyAfterCharacter(this string str, char c)
        {
            var split = str.Split(c);
            var returnString = split.Length == 1 ? split[0] : split[1];

            return returnString;
        }
    }
}