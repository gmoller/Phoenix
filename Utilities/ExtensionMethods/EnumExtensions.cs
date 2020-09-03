using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.ExtensionMethods
{
    public static class EnumExtensions
    {
        public static List<Enum> Values(this Enum theEnum)
        {
            return Enum.GetValues(theEnum.GetType()).Cast<Enum>().ToList();
        }

        public static Enum Max(this Enum theEnum)
        {
            return theEnum.Values().Max();
        }
    }
}