namespace Assets.ExtensionMethods
{
    public static class AtlasSpec2Extensions
    {
        public static bool HasValue(this AtlasSpec2 spec)
        {
            var hasValue = spec != null;

            return hasValue;
        }

        public static bool IsNull(this AtlasSpec2 spec)
        {
            var isNull = spec == null;

            return isNull;
        }
    }
}