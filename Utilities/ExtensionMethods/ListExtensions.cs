using System.Collections.Generic;

namespace Utilities.ExtensionMethods
{
    public static class ListExtensions
    {
        public static List<PointI> RemoveLast(this List<PointI> list, int count)
        {
            var returnList = new List<PointI>();
            for (var i = 0; i < list.Count - count; i++)
            {
                var item = list[i];
                returnList.Add(item);
            }
            //list.RemoveRange(list.Count - count, count);

            return returnList;
        }
    }
}