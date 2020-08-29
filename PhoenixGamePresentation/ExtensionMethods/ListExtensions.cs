using System.Collections.Generic;
using Utilities;

namespace PhoenixGamePresentation.ExtensionMethods
{
    public static class ListExtensions
    {
        public static List<Point> RemoveLast(this List<Point> list, int count)
        {
            var returnList = new List<Point>();
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